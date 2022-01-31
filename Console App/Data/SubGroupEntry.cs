using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
{
    /// <summary>
    /// When a lootgroup refers to another lootgroup, the entry itself can have it's own prob (flat probability)...
    /// ... count, or loot_prob_template, which is independent of the same attributes of the child group itself
    /// This class allows us to reflect that fact
    /// </summary>
    public class SubGroupEntry
    {
        /// <summary>
        /// Link to the Loot Group referenced in the entry
        /// </summary>
        public Group Group { get; set; }

        /// <summary>
        /// The Count value of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public Range? Count { get; set; }

        /// <summary>
        /// The Probability of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public decimal? Prob { get; set; }

        /// <summary>
        /// The Probability Template of the ENTRY (NOT OF THE GROUP ITSELF!)
        /// </summary>
        public ProbTemplate? ProbTemplate { get; set; }
    }
}
