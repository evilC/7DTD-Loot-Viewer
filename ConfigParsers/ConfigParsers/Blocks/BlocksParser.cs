﻿using ConfigParsers.Common;
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
        // If Localization.xml was parsed, and Display Names obtained for the various blocks...
        // .. then this dictionary will hold a lookup table of container name (eg "cntStorageChest") to Display Names (eg "Wooden Chest")
        private Dictionary<string, string>? _displayNames;

        public BlocksParser(Dictionary<string, string>? displayNames)
        {
            _displayNames = displayNames;
        }

        public SortedDictionary<string, HashSet<string>> GetLootLists(string configFilePath)
        {
            var blocksFile = Path.Combine(new string[] { configFilePath, "blocks.xml" });
            // Key is Container (effectively a LootGroup) name (eg "playerStorage")
            // Value is list of names for that Container Group.
            // Values Can be EITHER game identifier name (eg "cntStorageChest")...
            // OR Display Name from Localization.txt (eg "Wooden Chest"), if available
            var lootLists = new SortedDictionary<string, HashSet<string>>();
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
                                lootLists[property.Value] = new HashSet<string>();
                            }
                            string name;
                            if (_displayNames != null && _displayNames.ContainsKey(rawBlock.Name))
                            {
                                name = _displayNames[rawBlock.Name];
                            }
                            else
                            {
                                name = rawBlock.Name;
                            }
                            lootLists[lootList].Add(name);
                            //continue; // Do not process any other properties
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
