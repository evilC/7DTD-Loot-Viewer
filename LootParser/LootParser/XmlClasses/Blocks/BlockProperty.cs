using System.Xml.Serialization;

namespace LootParser.XmlClasses.Blocks
{
    public class BlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
