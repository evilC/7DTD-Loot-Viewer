using System.Xml.Serialization;

/// <summary>
/// XML lootcontainer nodes deserialize into this class
/// </summary>
namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class Container
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Entries { get; set; }
    }
}
