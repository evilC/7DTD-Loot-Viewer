using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    public class Container
    {
        public string Name { get; set; }
        public decimal Prob { get; set; }

        public Container(string name, decimal prob)
        {
            Name = name;
            Prob = prob;
        }
    }
}
