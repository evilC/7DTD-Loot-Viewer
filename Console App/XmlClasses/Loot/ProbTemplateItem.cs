using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class ProbTemplateItem
    {
        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("prob")]
        public decimal Prob { get; set; }
    }
}
