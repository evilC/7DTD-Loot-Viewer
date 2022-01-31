using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    public class LootContainer
    {
        /// <summary>
        /// The name of the Container (eg passngasCrate)
        /// Maps to an XML lootcontainer node
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Groups that the Container contains
        /// </summary>
        public Dictionary<string, LootGroup> Groups { get; set; } = new Dictionary<string, LootGroup>();

        /// <summary>
        /// Items that the Container contains
        /// </summary>
        public Dictionary<string, LootGroupItem> Items { get; set; } = new Dictionary<string, LootGroupItem>();
    }
}
