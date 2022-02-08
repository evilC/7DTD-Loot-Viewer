using ConfigParsers.Blocks.Data;
using ConfigParsers.Common;

namespace ConfigParsers.Blocks
{
    public class BlocksParser
    {
        /// <summary>
        /// Holds a representation of what is in the XML
        /// </summary>
        public SortedDictionary<string, Block> Blocks { get; private set; } = new SortedDictionary<string, Block>();

        // Used by the UI to translate LootList names (What appears in loot.xml) into Container Names (Using Localization.txt)
        // Key is Container (effectively a LootGroup) name (eg "playerStorage")
        // Value is list of names for that Container Group.
        // Value is game identifier name (eg "cntStorageChest")...
        public SortedDictionary<string, List<string>> BlockList { get; private set; } = new SortedDictionary<string, List<string>>();
        
        public void LoadConfigFile(string configFilePath)
        {
            ParseXml(configFilePath);
            Build();
        }

        /// <summary>
        /// Parse the XML and build a Dictionary of objects that it contains
        /// </summary>
        /// <param name="configFilePath">The path to blocks.xml</param>
        private void ParseXml(string configFilePath)
        {
            var blocksFile = Path.Combine(new string[] { configFilePath, "blocks.xml" });
            if (!File.Exists(blocksFile)) return;
            BlockList = new SortedDictionary<string, List<string>>();
            var rawBlocks = ObjectDeserializer.DeserializeToObject<XmlClasses.Root>
                (blocksFile);

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
                            lootList = property.Value;
                            break;
                        case "Extends":
                            extends = property.Value;
                            break;
                    }
                }

                foreach (var drop in rawBlock.Drops)
                {
                    switch (drop.Event)
                    {
                        case "Harvest":
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
                            drops.Add(new BlockDrop(drop.ResourceName, prob, dropType));
                            break;
                    }
                }

                Blocks.Add(rawBlock.Name, new Block(rawBlock.Name, extends, lootList, drops));
            }
        }

        /// <summary>
        /// Build lookup tables which will be used by the UI
        /// </summary>
        private void Build()
        {
            BlockList = new();
            foreach (var block in Blocks.Values)
            {
                if (block.LootList != null)
                {
                    if (!BlockList.ContainsKey(block.LootList))
                    {
                        BlockList.Add(block.LootList, new());
                    }
                    BlockList[block.LootList].Add(block.Name);
                }
            }
        }
    }
}
