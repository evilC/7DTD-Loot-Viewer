using _7DTD_Loot_Parser.XmlClasses.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    public class LootGroup
    {
        public string Name { get; set; }
        public Range? Count { get; set; }

        public Dictionary<string, LootGroupItem> Items { get; } = new Dictionary<string, LootGroupItem>();
        public Dictionary<string, LootGroup> Groups { get; } = new Dictionary<string, LootGroup>();

        public LootGroup(string name, Range? count)
        {
            Name = name;
            Count = count;
        }
    }
}
