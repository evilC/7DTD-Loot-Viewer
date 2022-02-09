using ConfigParsers.Blocks.Data;
using ConfigParsers.Common;
using System.Diagnostics;

namespace ConfigParsers.Blocks
{
    public class BlocksParser
    {
        /// <summary>
        /// Holds a representation of what is in the XML
        /// </summary>
        public SortedDictionary<string, Block> Blocks { get; private set; } = new();

        // Used by the UI to translate LootList names (What appears in loot.xml) into Container Names (Using Localization.txt)
        // Key is Container (effectively a LootGroup) name (eg "playerStorage")
        // Value is list of names for that Container Group.
        // Value is game identifier name (eg "cntStorageChest")...
        public SortedDictionary<string, List<string>> BlockList { get; private set; } = new SortedDictionary<string, List<string>>();

        private Count _zeroCount = new Count("0,0");

        public void LoadConfigFile(string configFilePath)
        {
            var blocks = ParseXml(configFilePath);
            var extendedBlocks = Build(blocks);
            Blocks = extendedBlocks;
        }

        /// <summary>
        /// Parse the XML and build a Dictionary of objects that it contains
        /// </summary>
        /// <param name="configFilePath">The path to blocks.xml</param>
        private SortedDictionary<string, Block> ParseXml(string blocksFile)
        {
            var blocks = new SortedDictionary<string, Block>();
            if (!File.Exists(blocksFile)) return blocks;
            BlockList = new SortedDictionary<string, List<string>>();
            var rawBlocks = ObjectDeserializer.DeserializeToObject<XmlClasses.Root>
                (blocksFile);

            var dropCount = 0;
            foreach (var rawBlock in rawBlocks.Blocks)
            {
                string? extends = null;
                string? lootList = null;
                var drops = new List<BlockDrop>();

                foreach (var property in rawBlock.Properties)
                {
                    switch (property.Name)
                    {
                        case "LootList":
                            if (lootList != null)
                            {
                                throw new Exception($"Block {rawBlock.Name} has multiple LootLists");
                            }
                            lootList = property.Value;
                            break;
                        case "Extends":
                            if (extends != null)
                            {
                                throw new Exception($"Block {rawBlock.Name} extends multiple blocks");
                            }
                            extends = property.Value;
                            break;
                    }
                }

                foreach (var drop in rawBlock.Drops)
                {
                    switch (drop.Event)
                    {
                        case "Harvest":
                            if (string.IsNullOrWhiteSpace(drop.ResourceName)) continue;
                            var prob = string.IsNullOrWhiteSpace(drop.Prob) ? 1 : Convert.ToDecimal(drop.Prob);
                            DropType dropType;
                            if (drop.Tag == "salvageHarvest")
                            {
                                dropType = DropType.Salvage;
                            }
                            else if (drop.Tag == "allHarvest")
                            {
                                dropType = DropType.Harvest;
                            }
                            else
                            {
                                continue;
                            }
                            drops.Add(new BlockDrop(drop.ResourceName, new Count(drop.Count), prob, dropType));
                            break;
                    }
                }
                dropCount += drops.Count;
                blocks.Add(rawBlock.Name, new Block(rawBlock.Name, extends, lootList, drops));
            }
            Debug.WriteLine($"Original drop count: {dropCount}");
            return blocks;
        }

        /// <summary>
        /// Build lookup tables which will be used by the UI
        /// </summary>
        private SortedDictionary<string, Block> Build(SortedDictionary<string, Block> originalBlocks)
        {
            var dropCount = 0;
            BlockList = new();
            var extendedBlocks = new SortedDictionary<string, Block>();
            foreach (var block in originalBlocks.Values)
            {
                if (block.LootList != null)
                {
                    if (!BlockList.ContainsKey(block.LootList))
                    {
                        BlockList.Add(block.LootList, new());
                    }
                    BlockList[block.LootList].Add(block.Name);
                }
                // ToDo: It seems like getting extended drops is will not add in any drops...
                // ... see comments in dupe checking for GetExtendedDrops - it seems that nothing is ever added
                // However, it seems that we sometimes have to REMOVE drops!
                // A BlockDrop can have a count of 0...
                // ... This seems to be used when the Block that it extends has a non-zero Count...
                // ... so the extending Block can REMOVE drops from the Block(s) it extends
                var extendedBlock = GetExtendedDrops(originalBlocks, block, null);
                extendedBlocks.Add(extendedBlock.Name, extendedBlock);
                dropCount += extendedBlock.Drops.Count;
            }
            Debug.WriteLine($"Extended drop count: {dropCount}");
            return extendedBlocks;
        }

        /// <summary>
        /// For a given block, gets any drops from blocks that it extends
        /// </summary>
        /// <param name="thisBlock">The block to process</param>
        /// <param name="extendedBlock">Used for recursion, pass null when calling</param>
        /// <returns></returns>
        private Block GetExtendedDrops(SortedDictionary<string, Block> originalBlocks,
            Block thisBlock,
            Block? extendedBlock    // A block containing all the drops from the original block and all blocks that it extends
        )
        {
            if (extendedBlock == null)
            {
                // First call of this method (Root of recursion)
                // thisBlock will be the original block
                // Duplicate original block to extendedBlock
                extendedBlock = new Block(thisBlock.Name,
                    thisBlock.Extends,
                    thisBlock.LootList, 
                    new List<BlockDrop>(thisBlock.Drops)
                );
            }
            else
            {
                // Add drops from this block to the extendedBlock
                foreach (var extendedDrop in thisBlock.Drops)
                {
                    // Do not add drops which are exact duplicates of an existing drop
                    var dupe = false;
                    foreach (var originalDrop in extendedBlock.Drops)
                    {
                        if (originalDrop.ResourceName == extendedDrop.ResourceName
                            && originalDrop.Prob == extendedDrop.Prob
                            && originalDrop.DropType == extendedDrop.DropType)
                        {
                            // Extending drop may be identical to base drop.
                            // Typically, this seems to be the case when the extending drop has a count of 0...
                            // ... and the base count has a non-zero value
                            // Doesn't seem like we have to take any action
                            dupe = true;
                            break;
                        }
                    }
                    if (!dupe)
                    {
                        // This never seems to happen, so it seems that this function does not do anything useful.
                        Debug.WriteLine($"Adding {extendedDrop.ResourceName} from {thisBlock.Name} to {extendedBlock.Name}");
                        extendedBlock.Drops.Add(extendedDrop);
                    }
                }
            }
            // If this block extends another block, then add drops from the extended block
            if (!string.IsNullOrWhiteSpace(thisBlock.Extends))
            {
                if (!originalBlocks.ContainsKey(thisBlock.Extends))
                {
                    throw new Exception($"Block {thisBlock.Name} extends non-existant block {thisBlock.Extends}");
                }
                var nextBlock = originalBlocks[thisBlock.Extends];
                GetExtendedDrops(originalBlocks, nextBlock, extendedBlock);
            }

            // Now that we have processed extended blocks, we can filter out drops with a count of 0
            for (int i = extendedBlock.Drops.Count-1; i >= 0; i--)
            {
                var drop = extendedBlock.Drops[i];
                if (drop.Count.Equals(_zeroCount))
                {
                    extendedBlock.Drops.RemoveAt(i);
                }
            }
            return extendedBlock;
        }
    }
}
