using _7DTD_Loot_Parser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// Parses the Raw XML classes, and attempts to build a more useful data structure from it...
    /// ... where all items refer to other items, allowing you to walk the tree
    /// </summary>
    public class Loot
    {
        /// <summary>
        /// Loot Groups. Essentially the contents of all lootgroup nodes.
        /// Note however that one loot group can refer to another loot group
        /// </summary>
        public SortedDictionary<string, Group> Groups { get; set; } = new SortedDictionary<string, Group>();

        /// <summary>
        /// Probability Templates
        /// </summary>
        public SortedDictionary<string, ProbTemplate> Templates { get; set; } = new SortedDictionary<string, ProbTemplate>();

        /// <summary>
        /// Containers
        /// </summary>
        public SortedDictionary<string, Container> Containers { get; set; } = new SortedDictionary<string, Container> ();

        /// <summary>
        /// Items
        /// </summary>
        public SortedDictionary<string, Item>  Items { get; set; } = new SortedDictionary<string, Item> ();

        /// <summary>
        /// Data table is built upon instantiation of the class
        /// </summary>
        /// <param name="rawRoot">The deserialized XML document</param>
        public Loot(XmlClasses.Loot.Root rawRoot)
        {
            // Iterate through all lootprobtemplate nodes in the XML
            // We do this before iterating groups, as groups reference probability templates
            var probTemplates = rawRoot.LootProbTemplateBase[0].LootProbTemplates;
            foreach (var template in probTemplates)
            {
                Templates.Add(template.Name, new ProbTemplate(template));
            }

            // Iterate through all lootqualitytemplate nodes in the XML
            // Some Items (eg ammo9mmBulletBall in groupAmmoRegularGunslingerT1) use a Quality Template as a Probability Template...
            // ... so even if not trying to calculate Quality, we need to add the Quality Templates to the Probability Template list
            foreach (var qualTemplateBase in rawRoot.LootQualTemplateBase)
            {
                foreach (var template in qualTemplateBase.LootQualTemplates)
                {
                    Templates.Add(template.Name, new ProbTemplate(template));
                }

            }

            // Iterate through all lootgroup nodes
            // Each lootgroup node can reference Items or (possibly nested) Groups, as well as Probability / Quality templates
            var lootGroups = rawRoot.Groups;
            foreach (var group in lootGroups)
            {
                AddGroup(group.Name, rawRoot.GroupsDictionary);
            }

            // Iterate through all container nodes
            // Each container node can reference Items or (possibly nested) Groups, as well as Probability / Quality templates
            var containers = rawRoot.Containers;
            foreach (var container in containers)
            {
                AddContainer(container);
            }
        }

        private void AddContainer(XmlClasses.Loot.Container rawContainer)
        {
            var container = new Container();
            container.Name = rawContainer.Name;
            for (int i = 0; i < rawContainer.Entries.Count(); i++)
            {
                var entry = rawContainer.Entries[i];
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    var item = AddItem(entry);
                }
                else if (!string.IsNullOrEmpty(entry.Group))
                {
                    if (!Groups.ContainsKey(entry.Group))
                    {
                        throw new Exception($"Container {container.Name} references a non-existant group");
                    }
                    container.Groups.Add(entry.Group, Groups[entry.Group]);
                }
                else
                {
                    throw new FormatException($"Container entry {rawContainer.Name} item {i} has neither a Name nor Group");
                }
            }
            Containers.Add(container.Name, container);
        }

        /// <summary>
        /// Recursive function to process groups
        /// If the group contains a reference to another group, this function will recursively call itself
        /// </summary>
        /// <param name="groupName">The name of the group to process</param>
        /// <param name="groupsDictionary">The existing dictionary of groups which have been processed</param>
        /// <returns>The processed loot group</returns>
        /// <exception cref="FormatException">Should never happen - Group entries should always contain Name (Item) or Group</exception>
        private Group AddGroup(string groupName, Dictionary<string, XmlClasses.Loot.Group> groupsDictionary)
        {
            var rawGroup = groupsDictionary[groupName];
            Range? groupCount;
            if (rawGroup.Count == null)
            {
                groupCount = null;
            }
            else
            {
                groupCount = Parsers.ParseRange(rawGroup.Count);
            }

            Group group;
            if (Groups.ContainsKey(rawGroup.Name))
            {
                // Group has already been added, skip
                // This may happen if a previously processed group referenced this group
                return Groups[rawGroup.Name];
            }
            else
            {
                // First time this group was encountered
                group = new Group(rawGroup.Name, groupCount);
                Groups.Add(rawGroup.Name, group);
            }

            // Iterate through all Entries in the Group
            for (int i = 0; i < rawGroup.Entries.Count(); i++)
            {
                var rawEntry = rawGroup.Entries[i];
                if (!string.IsNullOrEmpty(rawEntry.Name))
                {
                    // Single item
                    if (group.Items.ContainsKey(rawEntry.Name))
                    {
                        // ToDo: ammo9mmBulletBall appears twice in groupAmmoRegularGunslingerT1...
                        // ... once with ProbTemplate QLTemplateT0 and once with QLTemplateT1
                        // Until I know how to deal with this, skip subsequent entries
                        continue;
                    }
                    try
                    {
                        var itemInstance = AddItem(rawEntry);
                        group.Items.Add(rawEntry.Name, itemInstance);
                    }
                    catch(KeyNotFoundException ex)
                    {
                        // ToDo: ammo9mmBulletBall in groupAmmoAdvancedGunslinger references Probability Template "T0", which does not exist
                        // Until I know if this is an error in the XML or not, I will have to throw an exception and catch it
                        continue;
                    }
                    
                }
                else if (!string.IsNullOrEmpty(rawEntry.Group))
                {
                    // Group (Which could contain other groups)
                    if (group.Groups.ContainsKey(rawEntry.Group))
                    {
                        // ToDo: groupArmorScaledTPlus contains groupArmorT2 twice ?!?
                        continue;
                    }
                    var childGroup = AddGroup(rawEntry.Group, groupsDictionary);
                    var subGroupEntry = new SubGroupEntry();
                    subGroupEntry.Group = childGroup;
                    subGroupEntry.Count = Parsers.ParseRange(rawEntry.Count);
                    subGroupEntry.ProbTemplate = rawEntry.ProbTemplate != null ? Templates[rawEntry.ProbTemplate] : null;
                    group.Groups.Add(rawEntry.Group, subGroupEntry);
                }
                else
                {
                    throw new FormatException ($"Loot Group entry {i} has neither a Name nor Group");
                }

            }
            return group;
        }

        /// <summary>
        /// Adds an item to the list of Items
        /// </summary>
        /// <param name="rawEntry"></param>
        /// <returns></returns>
        private ItemInstance AddItem(XmlClasses.Loot.Item rawEntry)
        {
            Item item;
            if (Items.ContainsKey(rawEntry.Name))
            {
                // Item already exists in the database, just add an instance of it
                item = Items[rawEntry.Name];
            }
            else
            {
                // First time this Item has been seen
                item = new Item(rawEntry.Name);
                Items.Add(rawEntry.Name, item);
            }
            var itemInstance = item.AddInstance(rawEntry, Templates);
            return itemInstance;
        }
    }
}
