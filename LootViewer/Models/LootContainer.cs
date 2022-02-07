using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    public class LootContainer
    {
        public string Name { get; set; }
        public decimal Prob { get; set; }

        public LootContainer(string name, decimal prob)
        {
            Name = name;
            Prob = prob;
        }
    }
}
