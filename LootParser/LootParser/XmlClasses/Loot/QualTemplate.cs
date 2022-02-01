using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LootParser.XmlClasses.Loot
{
    /// <summary>
    /// XML lootqualitytemplate nodes deserialize into this class
    /// At the moment, Quality is not properly processed...
    /// ... this class is only present because sometimes in loot.xml, a Quality Template is used as a Probability Template
    /// </summary>
    public class QualTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "qualitytemplate")]
        public List<QualTemplateItem> LootQualTemplateItems { get; set; }
    }
}
