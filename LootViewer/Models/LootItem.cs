using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    public class LootItem
    {
        public string DisplayName { get; }
        public string Name { get; }

        public LootItem(string displayName, string name)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
