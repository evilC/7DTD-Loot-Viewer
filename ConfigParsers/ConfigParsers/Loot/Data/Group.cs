﻿
namespace ConfigParsers.Loot.Data
{
    /// <summary>
    /// Allows this class to be used to refer to a Group or a Container
    /// This was done to simplify the code when walking back up the tree from an Item
    /// Each Group needs to store a reference to it's parent object...
    /// ... and this could be a Group or a Container ...
    /// ... So for now, the simplest solution is to use one type to handle both
    /// </summary>
    public enum GroupType { Group, Container }

    /// <summary>
    /// Refers to a single lootgroup node of the XML
    /// </summary>
    public class Group
    {
        /// <summary>
        /// The name attribute of the lootgroup node
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether this Group references a Group or a Container
        /// </summary>
        public GroupType GroupType { get; set; }

        /// <summary>
        /// Holds the data from the count attribute of the lootgroup node
        /// Could be a single value, or a range
        /// </summary>
        public Count Count { get; set; }

        /// <summary>
        /// A list of the item elements in the lootgroup node whose name was that of an individual item (eg Steel meleeToolPickT2SteelPickaxe)
        /// Indexed by name of Item
        /// </summary>
        public Dictionary<string, ItemInstance> Items { get; } = new Dictionary<string, ItemInstance>();

        /// <summary>
        /// Holds a list of Groups that this Group refers to
        /// </summary>
        /*
        <lootgroup name="groupSavageCountryCrate01">                        <-- This Group
            <item group="groupArmorScaled" loot_prob_template="low"/>       <-- This is a Reference. It has a prob_template of it's own
        </lootgroup>
        */
        public List<GroupReference> GroupReferences { get; set; } = new List<GroupReference>();

        /// <summary>
        /// Holds a list of Groups that refer to this Group
        /// </summary>
        /*
        <lootgroup name="groupSavageCountryCrate01">                        <-- The Parent Group
	        <item group="groupArmorScaled" loot_prob_template="low"/>       <-- This is the Reference. It points to the Parent Group
        </lootgroup>
        */
        public List<GroupReference> ParentGroupReferences { get; set; } = new List<GroupReference>();

        public Group(string name, Count count, GroupType groupType)
        {
            Name = name;
            Count = count;
            GroupType = groupType;
        }
    }
}
