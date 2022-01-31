using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    /// <summary>
    /// XML lootqualitytemplate qualitytemplate nodes deserialize into this class
    /// At the moment, Quality is not properly processed...
    /// ... this class is only present because sometimes in loot.xml, a Quality Template is used as a Probability Template
    /// </summary>
    public class QualTemplateItem
    {
        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("prob")]
        public decimal Prob { get; set; }
    }
}