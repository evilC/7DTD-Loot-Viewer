using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _7DTD_Loot_Parser.Utilities;

namespace _7DTD_Loot_Parser.Data
{

    public class ProbTemplate
    {
        private Dictionary<Range, decimal> _items = new Dictionary<Range, decimal>();
        public ProbTemplate(XmlClasses.Loot.ProbTemplate template)
        {
            foreach (var item in template.LootProbTemplateItems)
            {
                var range = Parsers.ParseRange(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                _items.Add((Range)range, Convert.ToDecimal(item.Prob));
            }
        }

        public ProbTemplate(XmlClasses.Loot.QualTemplate template)
        {
            foreach (var item in template.LootQualTemplateItems)
            {
                var range = Parsers.ParseRange(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                _items.Add((Range)range, Convert.ToDecimal(item.Prob));
            }
        }
    }
}
