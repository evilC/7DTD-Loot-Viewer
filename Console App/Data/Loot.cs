using _7DTD_Loot_Parser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    public class Loot
    {
        public Dictionary<string, LootGroup> Group { get; set; } = new Dictionary<string, LootGroup>();
        public Dictionary<string, ProbTemplate> Template { get; set; } = new Dictionary<string, ProbTemplate>();

        public Loot(XmlClasses.Loot.Root rawRoot)
        {
            var probTemplates = rawRoot.LootProbTemplateBase[0].LootProbTemplates;
            foreach (var template in probTemplates)
            {
                Template.Add(template.Name, new ProbTemplate(template));
            }

            // Some Items (eg ammo9mmBulletBall in groupAmmoRegularGunslingerT1) use a Quality Template as a Probability Template...
            // ... so even if not trying to calculate Quality, we need to add the Quality Templates to the Probability Template list
            foreach (var qualTemplateBase in rawRoot.LootQualTemplateBase)
            {
                foreach (var template in qualTemplateBase.LootQualTemplates)
                {
                    Template.Add(template.Name, new ProbTemplate(template));
                }

            }
            //var qualTemplates = rawRoot.LootQualTemplateBase[0].LootQualTemplates;

            var lootGroups = rawRoot.Groups;
            foreach (var group in lootGroups)
            {
                //Group.Add(group.Name, new LootGroup(group));
                AddGroup(group.Name, rawRoot.GroupsDictionary);
            }

        }

        private LootGroup AddGroup(string groupName, Dictionary<string, XmlClasses.Loot.Group> groupsDictionary)
        {
            //throw new NotImplementedException();
        //}

        //private LootGroup AddGroup(string groupName, List<XmlClasses.Loot.Group> lootGroups)
        //{
            var rawGroup = groupsDictionary[groupName];
            Range? groupCount;
            if (rawGroup.Count == null)
            {
                groupCount = null;
            }
            else
            {
                groupCount = Parsers.ParseRange(rawGroup.Count);
            }

            LootGroup group;
            if (Group.ContainsKey(rawGroup.Name))
            {
                var debug = "me"; // Can this happen?
                //group = Group[rawGroup.Name];
                return Group[rawGroup.Name];
            }
            else
            {
                group = new LootGroup(rawGroup.Name, groupCount);
                Group.Add(rawGroup.Name, group);
            }


            for (int i = 0; i < rawGroup.Items.Count(); i++)
            {
                var rawItem = rawGroup.Items[i];
                if (!string.IsNullOrEmpty(rawItem.Name))
                {
                    // Single item
                    if (group.Items.ContainsKey(rawItem.Name))
                    {
                        // ToDo: ammo9mmBulletBall appears twice in groupAmmoRegularGunslingerT1...
                        // ... once with ProbTemplate QLTemplateT0 and once with QLTemplateT1
                        // Until I know how to deal with this, skip subsequent entries
                        continue;
                    }
                    try
                    {
                        group.Items.Add(rawItem.Name, new LootGroupItem(rawItem, Template));
                    }
                    catch(KeyNotFoundException ex)
                    {
                        // ToDo: ammo9mmBulletBall in groupAmmoAdvancedGunslinger references Probability Template "T0", which does not exist
                        // Until I know if this is an error in the XML or not, I will have to throw an exception and catch it
                        continue;
                    }
                    
                }
                else if (!string.IsNullOrEmpty(rawItem.Group))
                {
                    // Group (Which could contain other groups)
                    if (group.Groups.ContainsKey(rawItem.Group))
                    {
                        // ToDo: groupArmorScaledTPlus contains groupArmorT2 twice ?!?
                        continue;
                    }
                    var childGroup = AddGroup(rawItem.Group, groupsDictionary);
                    group.Groups.Add(childGroup.Name, childGroup);
                }
                else
                {
                    throw new Exception($"Loot Group {rawGroup.Name} item {i} has neither a Name nor Group");
                }

            }
            return group;
        }
    }
}
