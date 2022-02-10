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
        public SortedDictionary<string, List<string>> LootList { get; private set; } = new();

        /// <summary>
        /// List of Items which can be harvested from Blocks
        /// Key is name of item, Value is the Item
        /// </summary>
        public SortedDictionary<string, HarvestItem> HarvestItems { get; } = new();

        private Count _zeroCount = new Count("0,0");

        public void LoadConfigFile(string configFilePath)
        {
            var blocks = ParseXml(configFilePath);
            var extendedBlocks = Build(blocks);
            Blocks = extendedBlocks;
            BuildItemHarvestList();
        }

        /// <summary>
        /// Parse the XML and build a Dictionary of objects that it contains
        /// </summary>
        /// <param name="configFilePath">The path to blocks.xml</param>
        private SortedDictionary<string, Block> ParseXml(string blocksFile)
        {
            var blocks = new SortedDictionary<string, Block>();
            if (!File.Exists(blocksFile)) return blocks;
            LootList = new SortedDictionary<string, List<string>>();
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
            LootList = new();
            var extendedBlocks = new SortedDictionary<string, Block>();
            foreach (var block in originalBlocks.Values)
            {
                if (block.LootList != null)
                {
                    if (!LootList.ContainsKey(block.LootList))
                    {
                        LootList.Add(block.LootList, new());
                    }
                    LootList[block.LootList].Add(block.Name);
                }
                // ToDo: It seems like getting extended drops is will not add in any drops...
                // ... see comments in dupe checking for GetExtendedDrops - it seems that nothing is ever added
                // However, it seems that we sometimes have to REMOVE drops!
                // A BlockDrop can have a count of 0...
                // ... This seems to be used when the Block that it extends has a non-zero Count...
                // ... so the extending Block can REMOVE drops from the Block(s) it extends
                //Debug.WriteLine($"Processing block: {block.Name}");
                var extendedBlock = GetExtendedDrops(originalBlocks, block, null);
                //Debug.WriteLine($"");
                extendedBlocks.Add(extendedBlock.Name, extendedBlock);
                dropCount += extendedBlock.Drops.Count;
            }
            //Debug.WriteLine($"Extended drop count: {dropCount}");
            return extendedBlocks;
        }

        /// <summary>
        /// For a given block, gets any drops from blocks that it extends
        /// Recursive function!
        /// When called on a leaf Block, burrows all the way to the root Block, and THEN begins processing
        /// ie even though you call it on the leaf Block, starts at the root Block and ends at the leaf Block
        /// </summary>
        /// <param name="thisBlock">The block to process</param>
        /// <param name="extendedBlock">Used for recursion, pass null when calling</param>
        /// <returns></returns>
        private Block GetExtendedDrops(SortedDictionary<string, Block> originalBlocks,
            Block thisBlock,
            Block? extendedBlock    // A block containing all the drops from the original block and all blocks that it extends
        )
        {
            if (string.IsNullOrWhiteSpace(thisBlock.Extends))
            {
                // Root block
                // First call of this method (Root of recursion)
                // thisBlock will be the original block
                // Duplicate original block to extendedBlock
                extendedBlock = new Block(thisBlock.Name,
                    thisBlock.Extends,
                    thisBlock.LootList,
                    new List<BlockDrop>(thisBlock.Drops)
                );
                foreach (var drop in thisBlock.Drops)
                {
                    if (drop.Count.Equals(_zeroCount))
                        throw new Exception("Zero drop count in root node");
                }
                //Debug.WriteLine($"Found Root block {thisBlock.Name}");
                return extendedBlock;
            }
            else
            {
                var nextBlock = originalBlocks[thisBlock.Extends];
                //Debug.WriteLine($"Recursing in: thisBlock={thisBlock.Name}");
                extendedBlock = GetExtendedDrops(originalBlocks, nextBlock, extendedBlock);
                // This line should only be hit once we have recursed all the way to the root
                //Debug.WriteLine($"Recursing out: thisBlock={thisBlock.Name}");
                if (!string.IsNullOrWhiteSpace(thisBlock.LootList))
                {
                    if (!string.IsNullOrWhiteSpace(thisBlock.LootList))
                        throw new Exception("LootList is already set");
                    extendedBlock.LootList = thisBlock.LootList;
                }

                // Iterate drops in the extending Block
                foreach (var extendingDrop in thisBlock.Drops)
                {
                    // Iterate drops in the extended Block
                    var isDupe = false;
                    for (int extendedIndex = extendedBlock.Drops.Count - 1; extendedIndex >= 0; extendedIndex--)
                    {
                        var extendedDrop = extendedBlock.Drops[extendedIndex];
                        if (extendingDrop.ResourceName == extendedDrop.ResourceName)
                        {
                            isDupe = true;
                            // Extending Block has same resource as extended Block
                            if (extendingDrop.Count.Equals(_zeroCount))
                            {
                                // Extending Block has a Drop of the same resource but with zero count...
                                // ... remove Drop from extended Block
                                //Debug.WriteLine($"Extending Block {thisBlock.Name} has resource {extendedDrop.ResourceName} already in extended block {extendedBlock.Name} but with 0 count - removing");
                                extendedBlock.Drops.RemoveAt(extendedIndex);
                            }
                            else
                            {
                                // Replace extended Drop with extending Drop
                                //Debug.WriteLine($"Replacing resource {extendingDrop.ResourceName} in extended Block {extendedBlock.Name} with version from extending Block {thisBlock.Name}");
                                extendedDrop = extendingDrop;

                            }
                            break;
                        }
                    }
                    if (!isDupe)
                    {
                        //Debug.WriteLine($"Extending Block {thisBlock.Name} has resource {extendingDrop.ResourceName} not present in extened Block {extendedBlock.Name}, adding");
                        if (extendingDrop.Equals(_zeroCount))
                            throw new Exception($"Not expecting extending Block {thisBlock.Name} to feature 0 count resource ({extendingDrop.ResourceName}) that is not in extended block {extendedBlock.Name}");
                        extendedBlock.Drops.Add(extendingDrop);
                    }
                }

            }
            extendedBlock.Name = thisBlock.Name;
            return extendedBlock;
        }

        private void BuildItemHarvestList()
        {
            foreach (var block in Blocks.Values)
            {
                if (block.Drops.Count > 0)
                {
                    foreach (var blockDrop in block.Drops)
                    {
                        HarvestItem item;
                        if (!HarvestItems.ContainsKey(blockDrop.ResourceName))
                        {
                            item = new HarvestItem(blockDrop.ResourceName);
                        }
                        else
                        {
                            item = HarvestItems[blockDrop.ResourceName];
                        }
                        var itemInstance = new HarvestItemInstance(blockDrop.Prob, blockDrop.DropType, block.Name);
                        item.Instances.Add(block.Name, itemInstance);
                    }

                }
            }
        }
    }
}
