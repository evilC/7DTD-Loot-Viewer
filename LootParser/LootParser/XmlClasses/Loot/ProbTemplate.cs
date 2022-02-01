using System.Xml.Serialization;

namespace LootParser.XmlClasses.Loot
{
    /// <summary>
    /// XML lootprobtemplate nodes deserialize into this class
    /// </summary>
    public class ProbTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "loot")]
        public List<ProbTemplateItem> LootProbTemplateItems { get; set; }
    }
}
