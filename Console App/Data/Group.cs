using _7DTD_Loot_Parser.XmlClasses.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
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
        /// Holds the data from the count attribute of the lootgroup node
        /// Could be a single value, or a range
        /// </summary>
        public Range? Count { get; set; }

        /// <summary>
        /// A list of the item elements in the lootgroup node whose name was that of an individual item (eg Steel meleeToolPickT2SteelPickaxe)
        /// Indexed by name of Item
        /// </summary>
        public Dictionary<string, Item> Items { get; } = new Dictionary<string, Item>();

        /// <summary>
        /// A list of the item elements in the lootgroup node whose name was that of another Group (eg groupToolsT2)
        /// indexed by name of Group
        /// </summary>
        public Dictionary<string, SubGroupEntry> Groups { get; } = new Dictionary<string, SubGroupEntry>();


        public Group(string name, Range? count)
        {
            Name = name;
            Count = count;
        }
    }
}
