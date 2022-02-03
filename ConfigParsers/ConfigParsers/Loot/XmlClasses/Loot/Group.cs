using System.Xml.Serialization;

/// <summary>
/// XML lootgroup nodes deserialize into this class
/// </summary>
namespace ConfigParsers.Loot.XmlClasses
{
    public class Group
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("count")]
        public string Count { get; set; } = string.Empty;

        [XmlElement(ElementName = "item")]
        public List<Item> Entries { get; set; } = new List<Item>();

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }
    }
}
