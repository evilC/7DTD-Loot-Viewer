using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// Classes which are used for Deserialization of the loot.xml file
/// These classes are only temporarily used...
/// ... a further pass is made to convert these "Raw" classes into something more easily processed
/// </summary>
namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    [XmlRoot(ElementName = "lootcontainers")]
    public class Root
    {
        [XmlElement(ElementName = "lootgroup")]
        public List<Group> Groups { get; set; }

        [XmlElement(ElementName = "lootcontainer")]
        public List<Container> Containers { get; set; }

        [XmlElement(ElementName = "lootprobtemplates")]
        public List<RootProbTemplate> LootProbTemplateBase { get; set; }
    }
}
