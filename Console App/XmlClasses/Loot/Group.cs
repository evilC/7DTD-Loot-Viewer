using System.Xml.Serialization;

/// <summary>
/// Classes which are used for Deserialization of the loot.xml file
/// These classes are only temporarily used...
/// ... a further pass is made to convert these "Raw" classes into something more easily processed
/// </summary>
namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class Group
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        //[XmlAttribute("count")]
        //public string Count { get; set; }

        //[XmlAttribute("loot_quality_template")]
        //public string QualityTemplate { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }

    }
}
