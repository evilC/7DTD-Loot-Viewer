using System.Xml.Serialization;

/// <summary>
/// XML lootgroup nodes deserialize into this class
/// </summary>
namespace LootParser.XmlClasses.Loot
{
    public class Group
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("count")]
        public string Count { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Entries { get; set; }

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }
    }
}
