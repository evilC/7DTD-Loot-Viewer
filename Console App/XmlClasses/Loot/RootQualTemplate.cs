using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class RootQualTemplate
    {
        [XmlElement(ElementName = "lootqualitytemplate")]
        public List<QualTemplate> LootQualTemplates { get; set; }
    }
}