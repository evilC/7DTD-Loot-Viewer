using ConfigParsers.Common;

namespace ConfigParsers.Blocks.Data
{
    public class HarvestItemInstance
    {
        public decimal Prob { get; }

        public DropType DropType { get; }

        public string Block { get; }

        public HarvestItemInstance(decimal prob, DropType dropType, string block)
        {
            Prob = prob;
            DropType = dropType;
            Block = block;
        }

    }
}
