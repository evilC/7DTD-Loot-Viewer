using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Blocks
{
    public class BlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
