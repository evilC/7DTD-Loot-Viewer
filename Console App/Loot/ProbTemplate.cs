using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Loot
{
    public class ProbTemplate
    {
        private Dictionary<Range, decimal> _items = new Dictionary<Range, decimal>();
        public ProbTemplate(XmlClasses.Loot.ProbTemplate template)
        {
            foreach (var item in template.LootProbTemplateItems)
            {
                var range = item.Level.Split(',');
                var rangeLow = Convert.ToInt32(range[0]);
                int rangeHigh;
                if (range.Count() > 1)
                {
                    rangeHigh = Convert.ToInt32(range[1]);
                }
                else
                {
                    rangeHigh = rangeLow;
                }
                
                _items.Add(new Range(rangeLow, rangeHigh), Convert.ToDecimal(item.Prob));
            }
        }
    }
}
