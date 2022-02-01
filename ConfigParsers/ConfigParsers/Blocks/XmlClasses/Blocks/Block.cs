using System.Xml.Serialization;

namespace ConfigParsers.Blocks.XmlClasses
{
    public class Block
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "property")]
        public List<BlockProperty> Properties { get; set; }
    }
}
