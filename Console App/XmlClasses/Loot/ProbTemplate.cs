using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class ProbTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "loot")]
        public List<ProbTemplateItem> LootProbTemplateItems { get; set; }
    }
}
