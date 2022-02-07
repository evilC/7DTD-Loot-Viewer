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
        public SortedDictionary<string, HashSet<string>> GetLootLists(string configFilePath)
        {
            var lootLists = new SortedDictionary<string, HashSet<string>>();
            var rawBlocks = ObjectDeserializer.DeserializeToObject<XmlClasses.Root>
                (Path.Combine(new string[] { configFilePath, "blocks.xml" }));

            foreach (var rawBlock in rawBlocks.Blocks)
            {
                foreach (var property in rawBlock.Properties)
                {
                    if (property.Name == "LootList")
                    {
                        var lootList = property.Value;
                        if (!lootLists.ContainsKey(property.Value))
                        {
                            lootLists[property.Value] = new HashSet<string>();
                        }
                        lootLists[lootList].Add(rawBlock.Name);
                        break;
                    }
                }
            }
            return lootLists;
        }

        public Dictionary<string, string> GetContainerNames(string configFilePath)
        {
            /*
            To find Display name for containers
            1) Scan blocks.xml to find the LootList entry for that group:

            blocks.xml:
            <block name="cntMedicineCabinetClosed">
                <property name="Extends" value="cntMedicineCabinetOpen"/>
                <property name="CreativeMode" value="Player"/>
                <property name="Class" value="Loot"/>
            --> <property name="LootList" value="medicineCabinet"/>
                <property name="Model" value="#Entities/LootContainers?medicine_cabinet_closedPrefab.prefab"/>
            </block>

            2) Find the localization value in localization.txt
            localization.txt:
            cntMedicineCabinetClosed,blocks,Container,,,"Medicine Cabinet, Closed"
            */

            var containerNames = new Dictionary<string, string>();

            var rawBlocks = ObjectDeserializer.DeserializeToObject<Blocks.XmlClasses.Root>
                (Path.Combine(new string[] { configFilePath, "blocks.xml" }));

            foreach (var rawBlock in rawBlocks.Blocks)
            {
                foreach (var property in rawBlock.Properties)
                {
                    if (property.Name == "LootList")
                    {
                        if (containerNames.ContainsKey(rawBlock.Name))
                        {
                            Debug.WriteLine($"Key {rawBlock.Name} already has a value of {containerNames[rawBlock.Name]}, not adding {property.Value}");
                            //Debug.WriteLine($"Container {property.Value} already exists as {containerNames[property.Value]}, not adding {rawBlock.Name}");
                        }
                        else
                        {
                            containerNames.Add(rawBlock.Name, property.Value);
                        }
                        break;
                    }
                }
            }

            return containerNames;
        }

    }
}
