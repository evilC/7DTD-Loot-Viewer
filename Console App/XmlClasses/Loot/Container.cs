using System.Xml.Serialization;

/// <summary>
/// Classes which are used for Deserialization of the loot.xml file
/// These classes are only temporarily used...
/// ... a further pass is made to convert these "Raw" classes into something more easily processed
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
