using ConfigParsers.Loot.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Loot
{
    public class ProbabilityCalculator
    {
        private ItemContainer _containerResult;

        public ProbabilityCalculator(ItemContainer containerResult)
        {
            _containerResult = containerResult;
        }

        public void CalculateProbability(int lootLevel)
        {
            var foundNonZeroProb = false;
            Debug.WriteLine($"Finding probability for loot level {lootLevel}...");
            Debug.WriteLine($"( --> indicates a valid path through to the item )\n");

            var pathCount = _containerResult.Paths.Count;
            Debug.WriteLine($"{pathCount} paths were found\n");

            for (int nl = 0; nl < pathCount; nl++)
            {
                var prob = CalculatePathProbability(nl, lootLevel);
                if (prob > 0) foundNonZeroProb = true;
                Debug.WriteLine($"");
            }
            if (foundNonZeroProb)
            {
                Debug.WriteLine($"Is possible");
            }
            else
            {
                Debug.WriteLine($"ALL PATHS HAVE ZERO PROBABILITY");
            }
        }

        public decimal CalculatePathProbability(int nl, int lootLevel)
        {
            var itemPath = _containerResult.Paths[nl];
            var nodeList = itemPath.Nodes;
            Debug.WriteLine($"PATH #{nl + 1}:");
            foreach (var node in nodeList)
            {
                Debug.WriteLine($"{node.Group.Render()}");
                var groupReferences = node.Group.GroupReferences;
                var validPath = node.GroupReferenceIndex;
                for (int i = 0; i < groupReferences.Count; i++)
                {
                    var str = i == validPath ? "--> " : "    ";
                    var groupReference = groupReferences[i];
                    // ToDo: Only ProbTemplate of GroupReference considered here, not Prob ?
                    var probTemplate = groupReference.ProbTemplate == null ? 1 : groupReference.ProbTemplate.GetProb(lootLevel);
                    str += $"(Group {groupReference.Group.Name}) {groupReference.Render()} >>> PROBABILITY = {probTemplate}";
                    Debug.WriteLine($"{str}");
                    if (i == validPath && probTemplate == 0) 
                    {
                        Debug.WriteLine($"PATH ABORTED, NOT POSSIBLE AT LOOT LEVEL {lootLevel}");
                        return 0;
                    }
                }
            }
            var lastGroup = nodeList.Last().Group;
            foreach (var itemInstance in lastGroup.Items.Values)
            {
                var itemName = itemInstance.Item.Name;
                var str = itemName == itemPath.ItemInstance.Item.Name ? "--> " : "    ";
                
                var probTemplate = itemInstance.ProbTemplate == null ? 1 : itemInstance.ProbTemplate.GetProb(lootLevel);
                str += $"{itemInstance.Item.Name} >>>  PROBABILITY = {itemInstance.GetProb(lootLevel)}";
                Debug.WriteLine($"{str}");
            }

            Debug.WriteLine($"\nProbability for path = ???");
            return 1;
        }
    }
}
