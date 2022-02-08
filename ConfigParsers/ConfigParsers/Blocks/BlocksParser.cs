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
        public BlocksParser()
        {
        }

        public SortedDictionary<string, List<string>> GetLootLists(string configFilePath)
        {
            var blocksFile = Path.Combine(new string[] { configFilePath, "blocks.xml" });
            // Key is Container (effectively a LootGroup) name (eg "playerStorage")
            // Value is list of names for that Container Group.
            // Value is game identifier name (eg "cntStorageChest")...
            var lootLists = new SortedDictionary<string, List<string>>();
            if (!File.Exists(blocksFile)) return lootLists;
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
                            if (!lootLists.ContainsKey(property.Value))
                            {
                                lootLists[property.Value] = new List<string>();
                            }
                            lootLists[lootList].Add(rawBlock.Name);
                            break;
                    }
                }
            }
            return lootLists;
        }
    }
}
