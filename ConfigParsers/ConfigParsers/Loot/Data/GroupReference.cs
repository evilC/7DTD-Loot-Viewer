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

        public GroupReference(Group group, Group parent, int parentGroupReferenceIndex, Count count, decimal? prob, ProbTemplate? probTemplate)
        {
            Group = group;
            Parent = parent;
            ParentGroupReferenceIndex = parentGroupReferenceIndex;
            Count = count;
            Prob = prob;
            ProbTemplate = probTemplate;
        }

        /// <summary>
        /// Used for debugging output - renders attributes of this class
        /// </summary>
        public string Render()
        {
            var str = $"GroupReference: count={Count.Render()}";
            if (Prob != null) str += $", prob={Prob}";
            if (ProbTemplate != null) str += $", prob_template={ProbTemplate.Name}";
            return str;
        }

    }

}
