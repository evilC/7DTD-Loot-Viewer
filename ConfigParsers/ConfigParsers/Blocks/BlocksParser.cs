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


        public void LoadConfigFile(string configFilePath)
        {
            var blocksFile = Path.Combine(new string[] { configFilePath, "blocks.xml" });
            if (!File.Exists(blocksFile)) return;
            BlockList = new SortedDictionary<string, List<string>>();
            var rawBlocks = ObjectDeserializer.DeserializeToObject<XmlClasses.Root>
                (blocksFile);

            foreach (var rawBlock in rawBlocks.Blocks)
            {
                foreach (var property in rawBlock.Properties)
                {
                    switch (property.Name)
                    {
                        case "LootList":
                            var lootList = property.Value;
                            if (!BlockList.ContainsKey(property.Value))
                            {
                                BlockList[property.Value] = new List<string>();
                            }
                            BlockList[lootList].Add(rawBlock.Name);
                            break;
                    }
                }
            }
        }

        //public SortedDictionary<string, List<string>> GetLootLists(string configFilePath)
        //{

        //    return lootLists;
        //}
    }
}
