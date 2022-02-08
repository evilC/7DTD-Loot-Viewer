using ConfigParsers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Blocks.Data
{
    public class BlockDrop
    {
        public string ResourceName { get; }
        public decimal Prob { get; }
        public DropType DropType { get; }

        public BlockDrop(string resourceName, decimal prob, DropType dropType)
        {
            ResourceName = resourceName;
            Prob = prob;
            DropType = dropType;
        }
    }
}
