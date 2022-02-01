using System.Xml.Serialization;

namespace ConfigParsers.Blocks.XmlClasses
{
    public class BlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
