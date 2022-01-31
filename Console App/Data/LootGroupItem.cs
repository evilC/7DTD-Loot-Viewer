using _7DTD_Loot_Parser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    public class LootGroupItem
    {
        public string Name { get; set; }
        public Range? Count { get; set; }
        public ProbTemplate? ProbTemplate { get; set; }
        public decimal? Prob { get; set; }

        public LootGroupItem(XmlClasses.Loot.Item rawItem, Dictionary<string, ProbTemplate> probTemplates)
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
