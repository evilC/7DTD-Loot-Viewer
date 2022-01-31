using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
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
