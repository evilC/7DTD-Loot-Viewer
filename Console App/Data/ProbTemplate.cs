using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _7DTD_Loot_Parser.Utilities;

namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// Holds data which allows one to calulate probability of something dropping
    /// Can be an attribute of an Item or a Group
    /// </summary>
    public class ProbTemplate
    {
        /// <summary>
        /// The Name of the Template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The entries in the Template
        /// Maps to the XML "loot" elements
        /// </summary>
        public Dictionary<Range, decimal> Entries = new Dictionary<Range, decimal>();

        public ProbTemplate(XmlClasses.Loot.ProbTemplate template)
        {
            Name = template.Name;
            foreach (var item in template.LootProbTemplateItems)
            {
                var range = Parsers.ParseRange(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                Entries.Add((Range)range, Convert.ToDecimal(item.Prob));
            }
        }

        public ProbTemplate(XmlClasses.Loot.QualTemplate template)
        {
            Name = template.Name;
            foreach (var item in template.LootQualTemplateItems)
            {
                var range = Parsers.ParseRange(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                Entries.Add((Range)range, Convert.ToDecimal(item.Prob));
            }
        }
    }
}
