using System.Xml.Serialization;

namespace ConfigParsers.Blocks.XmlClasses
{
    public class BlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}
