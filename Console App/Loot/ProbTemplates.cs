using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Loot
{
    public class ProbTemplates
    {
        private Dictionary<string, ProbTemplate> _templates = new Dictionary<string,ProbTemplate>();

        public ProbTemplate this[string name]
        {
            get => _templates[name];
        }

        public ProbTemplates(XmlClasses.Loot.Root rawRoot)
        {
            var probTemplates = rawRoot.LootProbTemplateBase[0].LootProbTemplates;
            foreach (var template in probTemplates)
            {
                _templates.Add(template.Name, new ProbTemplate(template));
            }
        }
    }
}
