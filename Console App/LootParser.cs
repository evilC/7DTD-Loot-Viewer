using System.Xml.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace _7DTD_Loot_Parser
{
    /// <summary>
    /// Parses the 7DTD loot.xml file, and build two lists...
    /// The ContainerItems list is a list of which Items can spawn in each Container, indexed by Container Name
    /// The ItemContainers list is a list of which Containers each Item can spawn in, indexed by Item Name
    /// </summary>
    internal class LootParser
    {
        private XmlClasses.Loot.Root _rawContainers;
        private Dictionary<string, XmlClasses.Loot.Group> _rawGroups;
        private Dictionary<string, string> _itemNames;

        public LootParser(string configFilePath)
        {
            var localizationParser = new LocalizationParser();
            _itemNames = localizationParser.GetDisplayNames(configFilePath);

            // Deserialize the XML file into the Raw classes
            _rawContainers = ObjectDeserializer.DeserializeToObject<XmlClasses.Loot.Root>
                (Path.Combine(new string[] { configFilePath, "loot.xml" }));
            _rawContainers.BuildGroupDictionary();
            var loot = new Data.Loot(_rawContainers);

            //var probTemplates = new Loot.ProbTemplates(_rawContainers);
            return;

            /*
            // Convert the Loot Groups into a Dictionary, indexed by name of Loot Group
            //_rawGroups = _rawContainers.Groups.ToDictionary(i => i.Name);

            // Start building the loot table
            var lootTable = new Loot.Table();
            var containerItems = lootTable.ContainerItems;
            var itemContainers = lootTable.ItemContainers;
            // Iterate through all of the containers, and find which items could spawn in them
            foreach (var rawContainer in _rawContainers.Containers)
            {
                var itemsInThisContainer = new HashSet<string>();
                containerItems.Add(rawContainer.Name, itemsInThisContainer);
                // The RawContainer will contain a list of entries...
                // ... An entry can either be an Item, or a Group (Which itself can contain Items or another Group)
                AddGroupContents(rawContainer.Entries, itemsInThisContainer);

                // Iterate through the list of items in this container...
                foreach (var item in itemsInThisContainer)
                {
                    // ... Ensure that this Item has an entry in the ItemContainers list...
                    if (!itemContainers.ContainsKey(item))
                    {
                        itemContainers.Add(item, new HashSet<string>());
                    }
                    // ... Add the name of this Container to the entry for this Item
                    itemContainers[item].Add(rawContainer.Name);

                }
            }

            // Write out the lists as JSON
            var opt = new JsonSerializerOptions() { WriteIndented = true};

            File.WriteAllText("ContainerItems.json", JsonSerializer.Serialize(containerItems, opt));
            File.WriteAllText("ItemContainers.json", JsonSerializer.Serialize(itemContainers, opt));
            */
        }

        /// <summary>
        /// Recursive function which can walk down the tree of item groups and return a single list of items from all of the groups
        /// </summary>
        /// <param name="entries">The list of items in this group</param>
        /// <param name="itemsInThisContainer">The list of items to be added to</param>
        private void AddGroupContents(List<XmlClasses.Loot.Item> entries, HashSet<string> itemsInThisContainer)
        {
            foreach (var entry in entries)
            {
                if (entry.Name != null)
                {
                    // Entry is a single Item ... Add item to container
                    //itemsInThisContainer.Add(entry.Name);
                    if (_itemNames.ContainsKey(entry.Name))
                    {
                        itemsInThisContainer.Add(_itemNames[entry.Name]);
                    }
                    else
                    {
                        // Only Seems to happen for "Loadout Tier X"
                        Debug.WriteLine($"Warning! Item Name for {entry.Name} not found!");
                    }
                    
                }
                else if (entry.Group != null)
                {
                    // Entry is a Group (Which could potentially contain other groups)
                    AddGroupContents(_rawGroups[entry.Group].Items, itemsInThisContainer);
                }
            }
        }
    }

}
