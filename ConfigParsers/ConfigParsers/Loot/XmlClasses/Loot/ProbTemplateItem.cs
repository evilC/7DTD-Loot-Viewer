using System.Xml.Serialization;

namespace ConfigParsers.Loot.XmlClasses
{
    /// <summary>
    /// XML lootprobtemplate loot elements deserialize into this class
    /// </summary>
    public class ProbTemplateItem
    {
        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("prob")]
        public decimal Prob { get; set; }
    }
}
