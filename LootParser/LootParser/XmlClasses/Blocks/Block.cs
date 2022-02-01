using System.Xml.Serialization;

namespace LootParser.XmlClasses.Blocks
{
    public class Block
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "property")]
        public List<BlockProperty> Properties { get; set; }
    }
}
