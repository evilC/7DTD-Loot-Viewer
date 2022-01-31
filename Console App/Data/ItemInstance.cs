using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// An Instance of an Item (ie an occurence of an Item in a Loot Group)
    /// </summary>
    public class ItemInstance
    {
        /// <summary>
        /// Link back up to the Item that this is an instance of
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Count value for this instance
        /// </summary>
        public Range? Count { get; set; }

        /// <summary>
        /// Probability Template for this instance
        /// </summary>
        public ProbTemplate? ProbTemplate { get; set; }
        public decimal? Prob { get; set; }

    }
}
