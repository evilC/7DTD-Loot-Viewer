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
                var group = node.Group;
                var groupReferences = group.GroupReferences;
                var validPath = node.GroupReferenceIndex;
                decimal probFactor = 1 / (decimal)(groupReferences.Count + group.Items.Count);
                for (int i = 0; i < groupReferences.Count; i++)
                {
                    var groupReference = groupReferences[i];
                    var baseProb = groupReference.GetProb(lootLevel);
                    var str = i == validPath ? "--> " : "    ";
                    var probStr = $"Base: {baseProb}, Adjusted: ";
                    decimal prob;
                    if (group.Count.IsAll)
                    {
                        if (groupReference.ForceProb)
                        {
                            prob = baseProb;
                            probStr += $"(All, forced) ";
                        }
                        else
                        {
                            prob = 1;
                            probStr += $"(All, not forced) ";
                        }
                    }
                    else
                    {
                        // If the count of the group is not "all", then probability will be based upon the probability of all items in the group
                        // ToDo: If Count > 1, then we need to multiply by Count? How does this work eg for Count of 1,3 ?
                        // Also, what happens if a group's count is not set?
                        prob = groupReference.GetProb(lootLevel) * probFactor;
                    }
                    str += $"(Group {groupReference.Group.Name}) {groupReference.Render()} >>> PROBABILITY = {probStr}{prob}";
                    Debug.WriteLine($"{str}");
                    if (i == validPath && prob == 0) 
                    {
                        Debug.WriteLine($"PATH ABORTED, NOT POSSIBLE AT LOOT LEVEL {lootLevel}");
                        return 0;
                    }
                }
            }
            // Calculate probabilities for items in final group
            // ToDo: Adjusted values not calculated
            Debug.WriteLine($"ToDo: Adjusted values not calculated yet");
            var lastGroup = nodeList.Last().Group;
            foreach (var itemInstance in lastGroup.Items.Values)
            {
                var itemName = itemInstance.Item.Name;
                var str = itemName == itemPath.ItemInstance.Item.Name ? "--> " : "    ";
                
                str += $"{itemInstance.Item.Name} >>>  PROBABILITY = {itemInstance.GetProb(lootLevel)}";
                Debug.WriteLine($"{str}");
            }

            Debug.WriteLine($"\nProbability for path = ???");
            return 1;
        }
    }
}
