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
        public SortedDictionary<string, Group> Containers { get; set; } = new SortedDictionary<string, Group> ();

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
            // At this point, only add Groups, and not Group References
            foreach (var rawGroup in rawRoot.Groups)
            {
                var group = new Group(rawGroup.Name, Parsers.ParseRange(rawGroup.Count), GroupType.Group);
                Groups.Add(rawGroup.Name, group);
            }

            // Iterate through all container nodes
            // Add Groups to containers
            foreach (var rawGroup in rawRoot.Containers)
            {
                if (rawRoot.IsIgnoredContainer(rawGroup.Name)) continue;
                var group = new Group(rawGroup.Name, Parsers.ParseRange(rawGroup.Count), GroupType.Container);
                Groups.Add(rawGroup.Name, group);
                Containers.Add(rawGroup.Name, group);
                BuildGroup(rawGroup.Name, rawRoot.GroupsDictionary);
            }
        }

        private void BuildGroup(string groupName, Dictionary<string, XmlClasses.Loot.Group> rawGroups)
        {
            var rawGroup = rawGroups[groupName];
            var group = Groups[groupName];

            // If we have already built this group, then do not build it again
            if (group.Items.Count > 0 || group.GroupReferences.Count > 0) return;

            // Iterate through all Entries in the Group
            for (int i = 0; i < rawGroup.Entries.Count(); i++)
            {
                var rawEntry = rawGroup.Entries[i];
                if (!string.IsNullOrEmpty(rawEntry.Name))
                {
                    // Entry is an Item
                    if (group.Items.ContainsKey(rawEntry.Name))
                    {
                        // ToDo: ammo9mmBulletBall appears twice in groupAmmoRegularGunslingerT1...
                        // ... once with ProbTemplate QLTemplateT0 and once with QLTemplateT1
                        // Until I know how to deal with this, skip subsequent entries
                        continue;
                    }
                    try
                    {
                        var itemInstance = AddItem(rawEntry, group);
                        group.Items.Add(rawEntry.Name, itemInstance);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        // ToDo: ammo9mmBulletBall in groupAmmoAdvancedGunslinger references Probability Template "T0", which does not exist
                        // Until I know if this is an error in the XML or not, I will have to throw an exception and catch it
                        continue;
                    }

                }
                else if (!string.IsNullOrEmpty(rawEntry.Group))
                {
                    // Entry is a Group (Which could contain other groups)
                    var childGroup = Groups[rawEntry.Group];
                    // Build a reference that links this group and the childGroup
                    var childGroupReference = new GroupReference();
                    childGroupReference.Group = childGroup;
                    childGroupReference.Parent = group;
                    childGroupReference.Count = Parsers.ParseRange(rawEntry.Count);
                    childGroupReference.ProbTemplate = rawEntry.ProbTemplate != null ? Templates[rawEntry.ProbTemplate] : null;
                    // Add the reference to the child to this Group
                    group.GroupReferences.Add(childGroupReference);
                    // Add the reference to this Group to the child
                    childGroup.ParentGroupReferences.Add(childGroupReference);
                    // Build the Items and Groups in the child
                    BuildGroup(childGroup.Name, rawGroups);
                }
                else
                {
                    throw new FormatException($"Loot Group entry {i} has neither a Name nor Group");
                }

            }
        }

        /// <summary>
        /// Adds an item to the list of Items
        /// </summary>
        /// <param name="rawEntry"></param>
        /// <returns></returns>
        private ItemInstance AddItem(XmlClasses.Loot.Item rawEntry, Group group)
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

            var itemInstance = item.AddInstance(rawEntry, Templates, group);
            return itemInstance;
        }
    }
}
