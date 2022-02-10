using ConfigParsers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootViewer.Models
{
    public class LootList
    {
        public string Name { get; }
        public DropType DropType { get ; }
        public decimal Prob { get; }

        public LootList(string name, DropType dropType, decimal prob)
        {
            Name = name;
            DropType = dropType;
            Prob = prob;
        }
    }
}
