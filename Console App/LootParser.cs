using System.Xml.Serialization;
using System.Text.Json;

namespace _7DTD_Loot_Parser
{
    /// <summary>
    /// Parses the 7DTD loot.xml file, and build two lists...
    /// The ContainerItems list is a list of which Items can spawn in each Container, indexed by Container Name
    /// The ItemContainers list is a list of which Containers each Item can spawn in, indexed by Item Name
    /// </summary>
    internal class LootParser
    {
        private RawContainers _rawContainers;
        private Dictionary<string, RawGroup> _rawGroups;
        public LootParser(string filename = "loot.xml")
        {
            // Deserialize the XML file into the Raw classes
            _rawContainers = DeserializeToObject<RawContainers>(filename);
            // Convert the Loot Groups into a Dictionary, indexed by name of Loot Group
            _rawGroups = _rawContainers.Groups.ToDictionary(i => i.Name);

            // Start building the loot table
            var lootTable = new LootTable();
            var containerItems = lootTable.ContainerItems;
            var itemContainers = lootTable.ItemContainers;
            // Iterate through all of the containers, and find which items could spawn in them
            foreach (var rawContainer in _rawContainers.Containers)
            {
                var itemsInThisContainer = new HashSet<string>();
                containerItems.Add(rawContainer.Name, itemsInThisContainer);
                // The RawContainer will contain a list of entries...
                // ... An entry can either be an Item, or a Group (Which itself can contain Items or another Group)
                foreach (var entry in rawContainer.Entries)
                {
                    if (entry.Name != null)
                    {
                        // Entry is a single Item ... Add item to container
                        itemsInThisContainer.Add(entry.Name);
                    }
                    else if (entry.Group != null)
                    {
                        // Entry is a Group (Which could potentially contain other groups)
                        AddGroupContents(entry.Group, itemsInThisContainer);
                    }
                }

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
        }

        /// <summary>
        /// Recursive function which can walk down the tree of item groups and return a single list of items from all of the groups
        /// </summary>
        /// <param name="group">The name of the item group</param>
        /// <param name="containerItems">The list of items to be added to</param>
        private void AddGroupContents(string group, HashSet<string> containerItems)
        {
            var entries = _rawGroups[group].Items;
            foreach (var entry in entries)
            {
                if (entry.Name != null)
                {
                    // LootGroup item entry contains a single item
                    containerItems.Add(entry.Name);
                }
                else if (entry.Group != null)
                {
                    // LootGroup item entry contains another group...
                    // ... Recursively call self and pass existing list
                    AddGroupContents(entry.Group, containerItems);
                }
            }
        }

        public T DeserializeToObject<T>(string filepath) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }

}
