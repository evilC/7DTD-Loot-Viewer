using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser.Data
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
        public Group Group { get; set; }

        /// <summary>
        /// The Parent (owner) of the Reference
        /// </summary>
        public Group Parent { get; set; }

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
