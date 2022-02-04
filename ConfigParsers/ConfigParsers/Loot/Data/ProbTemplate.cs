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
        public string Name { get; }

        /// <summary>
        /// The entries in the Template
        /// Maps to the XML "loot" elements
        /// The key of the dictionary maps to the "level" attribute, and uses a Count class, as it does the same thing
        /// The value of the dictionary maps to the "prob" attribute
        /// </summary>
        //public SortedDictionary<Count, decimal> Entries = new SortedDictionary<Count, decimal>();
        public List<Tuple<Count, decimal>> Entries { get; set; } = new List<Tuple<Count, decimal>>();

        public ProbTemplate(XmlClasses.ProbTemplate template)
        {
            Name = template.Name;
            foreach (var item in template.LootProbTemplateItems)
            {
                var range = new Count(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                Entries.Add(new Tuple<Count, decimal>(range, Convert.ToDecimal(item.Prob)));
            }
        }

        public ProbTemplate(XmlClasses.QualTemplate template)
        {
            Name = template.Name;
            foreach (var item in template.LootQualTemplateItems)
            {
                var range = new Count(item.Level);

                if (range == null)
                {
                    throw new Exception("Not expecting null range");
                }
                Entries.Add(new Tuple<Count, decimal>(range, Convert.ToDecimal(item.Prob)));
            }
        }

        public decimal GetProb(int lootLevel)
        {
            foreach (var entry in Entries)
            {
                if (entry.Item1.IsInRange(lootLevel)) return entry.Item2;
            }
            return 0;
        }
    }
}
