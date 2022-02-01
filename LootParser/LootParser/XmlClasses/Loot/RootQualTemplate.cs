using System.Xml.Serialization;

namespace LootParser.XmlClasses.Loot
{
    /// <summary>
    /// XML lootqualitytemplates nodes deserialize into this class
    /// </summary>
    public class RootQualTemplate
    {
        [XmlElement(ElementName = "lootqualitytemplate")]
        public List<QualTemplate> LootQualTemplates { get; set; }
    }
}