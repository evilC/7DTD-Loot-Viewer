using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _7DTD_Loot_Parser.XmlClasses.Loot
{
    public class QualTemplate
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "qualitytemplate")]
        public List<QualTemplateItem> LootQualTemplateItems { get; set; }
    }
}
