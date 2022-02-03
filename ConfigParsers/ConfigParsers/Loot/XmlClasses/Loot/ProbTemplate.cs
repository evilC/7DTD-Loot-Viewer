using System.Xml.Serialization;

namespace ConfigParsers.Loot.XmlClasses
{
    /// <summary>
    /// XML lootprobtemplate nodes deserialize into this class
    /// </summary>
    public class ProbTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement(ElementName = "loot")]
        public List<ProbTemplateItem> LootProbTemplateItems { get; set; } = new List<ProbTemplateItem>();
    }
}
