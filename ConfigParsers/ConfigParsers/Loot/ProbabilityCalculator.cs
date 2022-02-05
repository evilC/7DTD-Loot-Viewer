using ConfigParsers.Loot.Data;
using System.Diagnostics;

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

                // Now we can divide a prob of 100% amongst the remaining probabilities
                // Unless the group's count is "all", in which case the prob of each entry is 1
                // Or if group's count is "all", and force_prob is true for a given item...
                // ...Then that item has that specific prob, and the remainder is divided amongst the other entries

                // Process GroupReferences (Sub-Groups)
                for (int i = 0; i < groupReferences.Count; i++)
                {
                    var groupReference = groupReferences[i];
                    var baseProb = groupReference.GetProb(lootLevel);
                    var forceProb = groupReference.ForceProb;
                    var str = i == validPath ? "--> " : "    ";
                    var prob = CalculateEntryProb($"{str}(Group {groupReference.Group.Name}) {groupReference.Render()}",
                        group.Count, probFactor, baseProb, forceProb, lootLevel);
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
                    if (itemInstance.Count.From == 0) throw new Exception("Code does not handle Items with a count From 0");
                    var prob = CalculateEntryProb(str, group.Count, probFactor, baseProb, forceProb, lootLevel);
                }

            }
            Debug.WriteLine($"\nProbability for path = ???");
            return 1;
        }

        /// <summary>
        /// Calculates the probability for an entry (GroupReference (sub-group) / Item)
        /// </summary>
        /// <param name="debugStr">Just used for debugging - what to print out at start of line</param>
        /// <param name="count">The Count value for the Group which this entry is in</param>
        /// <param name="probFactor">The multiplier for probabilities, taking into account other entries in the Group</param>
        /// <param name="baseProb">The base probability of whether this entry drops</param>
        /// <param name="forceProb">Whether force_prob is set for this entry</param>
        /// <param name="lootLevel">The current LootLevel</param>
        /// <returns></returns>
        private decimal CalculateEntryProb(string debugStr, Count count, decimal probFactor, decimal baseProb, bool forceProb, int lootLevel)
        {
            var probStr = $"Base: {baseProb}, Adjusted: ";
            decimal prob;
            if (count.IsAll)
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
                prob = baseProb * probFactor;
                if (forceProb) throw new Exception("force_prob encountered when group's count is not 'all'. This should never happen");
                else prob = count.AdjustProbForCount(prob);
            }
            Debug.WriteLine($"{debugStr} >>> PROBABILITY = {probStr}{prob}");
            return prob;
        }
    }
}
