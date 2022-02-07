using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    public class LootItem
    {
        public string Name { get; }

        public LootItem(string name)
        {
            Name = name;
        }
    }
}
