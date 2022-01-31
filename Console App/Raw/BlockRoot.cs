using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.RawClasses
{
    [XmlRoot(ElementName = "blocks")]
    public class BlockRoot
    {
        [XmlElement(ElementName = "block")]
        public List<Block> Blocks { get; set; }
    }

    public class Block
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "property")]
        public List<BlockProperty> Properties { get; set; }
    }

    public class BlockProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
