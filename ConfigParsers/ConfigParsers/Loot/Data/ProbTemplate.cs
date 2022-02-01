using ConfigParsers.Common;

namespace ConfigParsers.Loot.Data
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

        public ProbTemplate(XmlClasses.ProbTemplate template)
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

        public ProbTemplate(XmlClasses.QualTemplate template)
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
