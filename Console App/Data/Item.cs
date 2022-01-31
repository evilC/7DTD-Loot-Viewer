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
        public string Name { get; set; }
        public Range? Count { get; set; }
        public ProbTemplate? ProbTemplate { get; set; }
        public decimal? Prob { get; set; }

        public Item(XmlClasses.Loot.Item rawItem, SortedDictionary<string, ProbTemplate> probTemplates)
        {
            Name = rawItem.Name;
            Count = Parsers.ParseRange(rawItem.Count);
            if (!string.IsNullOrEmpty(rawItem.ProbTemplate))
            {
                if (!probTemplates.ContainsKey(rawItem.ProbTemplate))
                {
                    throw new KeyNotFoundException($"No such Probability Template {rawItem.ProbTemplate}");
                }
                ProbTemplate = probTemplates[rawItem.ProbTemplate];
            }

            if (!string.IsNullOrEmpty(rawItem.Prob))
            {
                Prob = Convert.ToDecimal(rawItem.Prob);
            }
        }
    }
}
