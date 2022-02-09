using ConfigParsers.Common;
using ConfigParsers.Loot.Data;
using System.Diagnostics;

namespace ConfigParsers.Loot
{
    /// <summary>
    /// Calculates the probability of an item dropping from a specific container
    /// </summary>
    
    // ToDo: Way more calculated than needs to be at the moment...
    // ...we do not need to calculate probabilities for entries not in the path
    // This was just done to make sure everything worked
    public class ProbabilityCalculator
    {
        private ItemContainer _containerResult;

        public bool DebugMode { get; set; } = false;

        public ProbabilityCalculator(ItemContainer containerResult)
        {
            _containerResult = containerResult;
        }

        public decimal CalculateProbability(int lootLevel)
        {
            var pathCount = _containerResult.Paths.Count;
            decimal probTotal = 0;
            if (DebugMode)
            {
                Debug.WriteLine($"Finding probability for loot level {lootLevel}...");
                Debug.WriteLine($"( --> indicates a valid path through to the item )\n");
                Debug.WriteLine($"{pathCount} paths were found\n");
            }

            for (int nl = 0; nl < pathCount; nl++)
            {
                var prob = CalculatePathProbability(nl, lootLevel);
                if (DebugMode) Debug.WriteLine($"Path probability: {prob}");
                probTotal += prob;
                if (DebugMode) Debug.WriteLine($"");
            }

            return probTotal;
        }

        public decimal CalculatePathProbability(int nl, int lootLevel)
        {
            decimal pathProbability = 1;
            var itemPath = _containerResult.Paths[nl];
            var nodeList = itemPath.Nodes;
            if (DebugMode) Debug.WriteLine($"PATH #{nl + 1}:");
            foreach (var node in nodeList)
            {
                if (DebugMode) Debug.WriteLine($"{node.Group.Render()}");
                var group = node.Group;
                var groupReferences = group.GroupReferences;
                var validPathIndex = node.GroupReferenceIndex;
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
                    var str = i == validPathIndex ? "--> " : "    ";
                    if (DebugMode) Debug.Write($"{str}(Group {groupReference.Group.Name})");
                    var prob = CalculateEntryProb(group.Count, probFactor, baseProb, forceProb, lootLevel);
                    if (i == validPathIndex)
                    {
                        if (i == validPathIndex)
                        {
                            if (prob == 0)
                            {
                                if (DebugMode) Debug.WriteLine($"PATH ABORTED, NOT POSSIBLE AT LOOT LEVEL {lootLevel}");
                                return 0;
                            }
                            pathProbability *= prob;
                        }
                    }
                }

                // Process Items in Group
                var itemProcessed = false;
                foreach (var itemInstance in group.Items.Values)
                {
                    var itemName = itemInstance.Item.Name;
                    var isItemPath = (itemName == itemPath.ItemInstance.Item.Name);
                    var str = isItemPath ? "--> " : "    ";
                    str += $"{itemInstance.Render()}";
                    if (DebugMode) Debug.Write(str);
                    var baseProb = itemInstance.GetProb(lootLevel);
                    var forceProb = itemInstance.ForceProb;
                    if (itemInstance.Count.From == 0) throw new Exception("Code does not handle Items with a count From 0");
                    var prob = CalculateEntryProb(group.Count, probFactor, baseProb, forceProb, lootLevel);
                    if (isItemPath)
                    {
                        //if (validPathIndex != null)
                        //{
                        //    // Seems to be OK?
                        //    throw new Exception("Not expecting both a GroupReference leading to the Item and the Item itself in the same Group");
                        //}
                        if (itemProcessed)
                        {
                            throw new Exception("Code does not handle scenarios where eg the same item appears twice in a Group's Items list (eg with two different counts)");
                        }
                        itemProcessed = true;

                        pathProbability *= prob;
                    }
                }

            }
            return pathProbability;
        }

        /// <summary>
        /// Calculates the probability for an entry (GroupReference (sub-group) / Item)
        /// </summary>
        /// <param name="count">The Count value for the Group which this entry is in</param>
        /// <param name="probFactor">The multiplier for probabilities, taking into account other entries in the Group</param>
        /// <param name="baseProb">The base probability of whether this entry drops</param>
        /// <param name="forceProb">Whether force_prob is set for this entry</param>
        /// <param name="lootLevel">The current LootLevel</param>
        /// <returns></returns>
        private decimal CalculateEntryProb(Count count, decimal probFactor, decimal baseProb, bool forceProb, int lootLevel)
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
            if (DebugMode) Debug.Write($" >>> PROBABILITY = {probStr}{prob}\n");
            return prob;
        }
    }
}
