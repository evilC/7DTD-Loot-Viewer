
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
        public string Name { get; }

        /// <summary>
        /// Whether this Group references a Group or a Container
        /// </summary>
        public GroupType GroupType { get; }

        /// <summary>
        /// Holds the data from the count attribute of the lootgroup node
        /// Could be a single value, or a range, or "all"
        /// If it is a single value or a range, then only that number of entries (Groups or Items) will be chosen
        /// If it is "all", then ALL groups or entries will be chosen
        /// </summary>
        public Count Count { get; }

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
        public List<GroupReference> GroupReferences { get; } = new List<GroupReference>();

        /// <summary>
        /// Holds a list of Groups that refer to this Group
        /// </summary>
        /*
        <lootgroup name="groupSavageCountryCrate01">                        <-- The Parent Group
	        <item group="groupArmorScaled" loot_prob_template="low"/>       <-- This is the Reference. It points to the Parent Group
        </lootgroup>
        */
        public List<GroupReference> ParentGroupReferences { get; } = new List<GroupReference>();

        public Group(string name, Count count, GroupType groupType)
        {
            Name = name;
            Count = count;
            GroupType = groupType;
        }

        /// <summary>
        /// Used for debugging output - renders attributes of this class
        /// </summary>
        public string Render()
        {
            return $"{GroupType}: name={Name}, count={Count.Render()}";
        }
    }
}
