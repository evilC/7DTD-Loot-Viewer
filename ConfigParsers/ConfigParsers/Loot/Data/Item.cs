
using ConfigParsers.Common;

namespace ConfigParsers.Loot.Data
{
    /// <summary>
    /// Refers to a specific in-game Item (eg Steel Pickaxe)
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The name of the item (eg meleeToolSalvageT2Ratchet)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// List of instances of this item
        /// </summary>
        public List<ItemInstance> Instances { get; } = new List<ItemInstance>();

        public Item(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawItem"></param>
        /// <param name="probTemplates"></param>
        /// <param name="group">The Group that this ItemInstance is in</param>
        /// <param name="instanceIndex">The index of this ItemInstance in the Group's Items array</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public ItemInstance AddInstance(XmlClasses.Item rawItem, SortedDictionary<string, ProbTemplate> probTemplates, Group group, int instanceIndex)
        {
            if (rawItem.ProbTemplate != null && !probTemplates.ContainsKey(rawItem.ProbTemplate))
            {
                throw new KeyNotFoundException($"No such Probability Template {rawItem.ProbTemplate}");
            }
            var instance = new ItemInstance(
                item: this,
                count: new Count(rawItem.Count),
                parentGroup: group,
                parentGroupItemIndex: instanceIndex,
                prob: string.IsNullOrEmpty(rawItem.Prob) ? null : Convert.ToDecimal(rawItem.Prob),
                probTemplate: string.IsNullOrEmpty(rawItem.ProbTemplate) ? null : probTemplates[rawItem.ProbTemplate]
            );

            Instances.Add(instance);
            return instance;
        }
    }
}
