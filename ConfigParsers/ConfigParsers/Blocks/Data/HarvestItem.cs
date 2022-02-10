
using ConfigParsers.Common;

namespace ConfigParsers.Blocks.Data
{
    public class HarvestItem
    {
        public string Name { get; }

        /// <summary>
        /// Key is name of Block for this instance
        /// </summary>
        public Dictionary<string, SortedDictionary<DropType, decimal>> Blocks { get; } = new();

        public HarvestItem(string name)
        {
            Name = name;
        }

        public void AddInstance(string blockName, BlockDrop blockDrop)
        {
            SortedDictionary<DropType, decimal> block;
            if (!Blocks.ContainsKey(blockName))
            {
                block = new SortedDictionary<DropType, decimal>();
                Blocks.Add(blockName, block);
            }
            else
            {
                block = Blocks[blockName];
            }

            if (!block.ContainsKey(blockDrop.DropType))
            {
                block.Add(blockDrop.DropType, blockDrop.Prob);
            }
            else
            {
                if (blockDrop.Prob > block[blockDrop.DropType])
                {
                    block[blockDrop.DropType] = blockDrop.Prob;
                }
            }
        }
    }
}
