
namespace ConfigParsers.Blocks.Data
{
    public class HarvestItem
    {
        public string Name { get; }

        /// <summary>
        /// Key is name of Block for this instance
        /// </summary>
        public Dictionary<string, HarvestItemInstance> Instances { get; } = new();

        public HarvestItem(string name)
        {
            Name = name;
        }
    }
}
