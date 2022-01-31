using _7DTD_Loot_Parser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// Refers to a specific in-game Item (eg Steel Pickaxe)
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The name of the item (eg meleeToolSalvageT2Ratchet)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of instances of this item
        /// </summary>
        public List<ItemInstance> Instances { get; set; } = new List<ItemInstance>();

        public Item(string name)
        {
            Name = name;
        }

        public ItemInstance AddInstance(XmlClasses.Loot.Item rawItem, SortedDictionary<string, ProbTemplate> probTemplates)
        {
            var instance = new ItemInstance();

            instance.Item = this;
            instance.Count = Parsers.ParseRange(rawItem.Count);
            if (!string.IsNullOrEmpty(rawItem.ProbTemplate))
            {
                if (!probTemplates.ContainsKey(rawItem.ProbTemplate))
                {
                    throw new KeyNotFoundException($"No such Probability Template {rawItem.ProbTemplate}");
                }
                instance.ProbTemplate = probTemplates[rawItem.ProbTemplate];
            }

            if (!string.IsNullOrEmpty(rawItem.Prob))
            {
                instance.Prob = Convert.ToDecimal(rawItem.Prob);
            }

            Instances.Add(instance);
            return instance;
        }
    }
}
