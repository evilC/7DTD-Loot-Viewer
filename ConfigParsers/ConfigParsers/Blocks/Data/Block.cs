using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Blocks.Data
{
    public class Block
    {
        public string Name { get;  }
        public string? LootList { get; }
        public string? Extends { get; }
        public List<BlockDrop> Drops { get; }

        public Block(string name, string? extends, string? lootList, List<BlockDrop> drops)
        {
            Name = name;
            Extends = extends;
            LootList = lootList;
            Drops = drops;
        }
    }
}
