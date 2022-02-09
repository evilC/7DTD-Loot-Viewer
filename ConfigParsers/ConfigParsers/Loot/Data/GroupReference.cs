using ConfigParsers.Common;

namespace ConfigParsers.Loot.Data
{
    /// <summary>
    /// When one Group references another, this class holds the reference
    /// </summary>
    /*
    <lootgroup name="groupSavageCountryCrate01">                        <-- The Parent Group (Which holds the reference)
	    <item group="groupArmorScaled" loot_prob_template="low"/>       <-- This is the Reference. It has a prob_template of it's own
    </lootgroup>
    */
    public class GroupReference
    {
        /// <summary>
        /// Link to the Loot Group referenced in the entry
        /// </summary>
        public Group Group { get; }

        /// <summary>
        /// The Parent (owner) of the Reference
        /// </summary>
        public Group Parent { get; }

        /// <summary>
        /// In the parent's GroupReferences list, what is the index of this item?
        /// </summary>
        public int ParentGroupReferenceIndex { get; }

        /// <summary>
        /// The Count value of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public Count Count { get; }

        /// <summary>
        /// The Probability of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public decimal? Prob { get; }

        /// <summary>
        /// The Probability Template of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public ProbTemplate? ProbTemplate { get; }

        /// <summary>
        /// Whether force_prob is set, true or false for this reference
        /// If containing Group has a Count value of "all"...
        /// Then all entries in it's list of Groups / Items will have a probability of 100%
        /// ForceProb can be used to override that for a specific entry
        /// ie
        /// group name="gp" count="all"
        ///     group gc1                               <-- will always pick one item from this group
        ///     group gc2                               <-- will always pick one item from this group
        ///     group gc3 force_prob="true" prob="0.5"  <-- will pick an item from this group 50% of the time
        /// 
        /// </summary>
        public bool ForceProb { get; }

        public GroupReference(Group group, Group parent, int parentGroupReferenceIndex, Count count, decimal? prob, ProbTemplate? probTemplate, bool? forceProb)
        {
            Group = group;
            Parent = parent;
            ParentGroupReferenceIndex = parentGroupReferenceIndex;
            Count = count;
            Prob = prob;
            ProbTemplate = probTemplate;
            ForceProb = forceProb == null ? false : (bool) forceProb;
        }

        public decimal GetProb(int lootLevel)
        {
            if (Prob != null) return (decimal)Prob;
            if (ProbTemplate != null) return ProbTemplate.GetProb(lootLevel);
            return 1;
        }

        /// <summary>
        /// Used for debugging output - renders attributes of this class
        /// </summary>
        public string Render()
        {
            var str = $"GroupReference: count={Count.Render()}";
            if (Prob != null) str += $", prob={Prob}";
            if (ProbTemplate != null) str += $", prob_template={ProbTemplate.Name}";
            str += $", force_prob={ForceProb}";
            return str;
        }

    }

}
