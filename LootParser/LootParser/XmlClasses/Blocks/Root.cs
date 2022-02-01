using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LootParser.XmlClasses.Blocks
{
    [XmlRoot(ElementName = "blocks")]
    public class Root
    {
        [XmlElement(ElementName = "block")]
        public List<Block> Blocks { get; set; }
    }
}
