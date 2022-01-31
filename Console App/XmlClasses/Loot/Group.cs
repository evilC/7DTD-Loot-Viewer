using System.Xml.Serialization;

/// <summary>
/// XML lootgroup nodes deserialize into this class
/// </summary>
namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class Group
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("count")]
        public string Count { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }
    }
}
