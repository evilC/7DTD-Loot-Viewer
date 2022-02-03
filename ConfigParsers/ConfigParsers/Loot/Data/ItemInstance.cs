using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Loot.Data
{
    /// <summary>
    /// An Instance of an Item (ie an occurence of an Item in a Loot Group)
    /// </summary>
    public class ItemInstance
    {
        /// <summary>
        /// Link back up to the Item that this is an instance of
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Count value for this instance
        /// </summary>
        public Count Count { get; }

        /// <summary>
        /// The parent Group of this instance
        /// </summary>
        public Group ParentGroup { get; }

        /// <summary>
        /// What the index is of this ItemInstance in the ParentGroup's Items list
        /// </summary>
        public int ParentGroupItemIndex { get; }

        /// <summary>
        /// Probability Template for this instance
        /// </summary>
        public ProbTemplate? ProbTemplate { get; }

        /// <summary>
        /// Probability for this instance
        /// </summary>
        public decimal? Prob { get; }

        public ItemInstance(Item item, Count count, Group parentGroup, int parentGroupItemIndex, decimal? prob, ProbTemplate? probTemplate)
        {
            Item = item;
            Count = count;
            ParentGroup = parentGroup;
            ParentGroupItemIndex = parentGroupItemIndex;
            Prob = prob;
            ProbTemplate = probTemplate;
        }
    }
}
