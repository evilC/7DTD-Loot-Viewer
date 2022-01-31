using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    public class Container
    {
        /// <summary>
        /// The name of the Container (eg passngasCrate)
        /// Maps to an XML lootcontainer node
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Groups that the Container contains
        /// </summary>
        public Dictionary<string, Group> Groups { get; set; } = new Dictionary<string, Group>();

        /// <summary>
        /// Items that the Container contains
        /// </summary>
        public Dictionary<string, Item> Items { get; set; } = new Dictionary<string, Item>();
    }
}
