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
                // First we need to filter out things with a prob of 0
                decimal probFactor = 0;
                for (int i = 0; i < groupReferences.Count; i++)
                {
                    var baseProb = groupReferences[i].GetProb(lootLevel);
                    if (baseProb > 0) probFactor += baseProb;
                }
                foreach (var itemInstance in group.Items.Values)
                {
                    var baseProb = itemInstance.GetProb(lootLevel);
                    if (baseProb > 0) probFactor += baseProb;
                }
                probFactor = 1 / probFactor;

                // Process GroupReferences (Sub-Groups)
                for (int i = 0; i < groupReferences.Count; i++)
                {
                    var groupReference = groupReferences[i];
                    var baseProb = groupReference.GetProb(lootLevel);
                    var forceProb = groupReference.ForceProb;
                    var str = i == validPath ? "--> " : "    ";
                    var prob = CalculateEntryProb($"{str}(Group {groupReference.Group.Name}) {groupReference.Render()}",
                        group.Count.IsAll, probFactor, baseProb, forceProb, lootLevel);
                    if (i == validPath && prob == 0) 
                    {
                        Debug.WriteLine($"PATH ABORTED, NOT POSSIBLE AT LOOT LEVEL {lootLevel}");
                        return 0;
                    }
                }
                // Process Items in Group
                foreach (var itemInstance in group.Items.Values)
                {
                    var itemName = itemInstance.Item.Name;
                    var str = itemName == itemPath.ItemInstance.Item.Name ? "--> " : "    ";
                    str += $"{itemInstance.Render()}";
                    var baseProb = itemInstance.GetProb(lootLevel);
                    var forceProb = itemInstance.ForceProb;
                    var prob = CalculateEntryProb(str, group.Count.IsAll, probFactor, baseProb, forceProb, lootLevel);
                }

            }
            Debug.WriteLine($"\nProbability for path = ???");
            return 1;
        }

        /// <summary>
        /// Calculates the probability for an entry (GroupReference (sub-group) / Item)
        /// </summary>
        /// <param name="debugStr">Just used for debugging - what to print out at start of line</param>
        /// <param name="group">The Group which this entry is in</param>
        /// <param name="baseProb">The base probability of whether this entry drops</param>
        /// <param name="forceProb">Whether force_prob is set for this entry</param>
        /// <param name="lootLevel">The current LootLevel</param>
        /// <returns></returns>
        private decimal CalculateEntryProb(string debugStr, bool isAll, decimal probFactor, decimal baseProb, bool forceProb, int lootLevel)
        {
            //decimal probFactor = 1 / (decimal)(group.GroupReferences.Count + group.Items.Count);
            var probStr = $"Base: {baseProb}, Adjusted: ";
            decimal prob;
            if (isAll)
            {
                if (forceProb)
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
                prob = baseProb * probFactor;
            }
            Debug.WriteLine($"{debugStr} >>> PROBABILITY = {probStr}{prob}");
            return prob;
        }
    }
}
