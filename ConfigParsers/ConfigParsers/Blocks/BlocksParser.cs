using ConfigParsers.Blocks.Data;
using ConfigParsers.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Blocks
{
    public class BlocksParser
    {
        // Key is Container (effectively a LootGroup) name (eg "playerStorage")
        // Value is list of names for that Container Group.
        // Value is game identifier name (eg "cntStorageChest")...
        public SortedDictionary<string, List<string>> BlockList { get; private set; } = new SortedDictionary<string, List<string>>();
        
        /// <summary>
        /// Holds a representation of what is in the XML
        /// </summary>
        public SortedDictionary<string, Block> Blocks { get; private set; } = new SortedDictionary<string, Block>();

        public void LoadConfigFile(string configFilePath)
        {
            ParseXml(configFilePath);
            Build();
        }

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
