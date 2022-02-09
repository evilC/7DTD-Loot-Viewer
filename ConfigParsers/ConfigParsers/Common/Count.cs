namespace ConfigParsers.Common
{
    /// <summary>
    /// Xml Count attributes are converted to this class, for easy processing
    /// </summary>
    public class Count : IEquatable<Count>
    {
        /// <summary>
        /// True if a count attribute was specified, otherwise false.
        /// If true, IsAll or From and To must be set
        /// </summary>
        //Deprecated - an unset count is assumed to be 1
        //public bool IsSet { get; } = false;

        /// <summary>
        /// True if the count attribute was "all"
        /// </summary>
        public bool IsAll { get; } = false;

        /// <summary>
        /// The From (1st) value of the count attribute
        /// </summary>
        public int From { get; }

        /// <summary>
        /// The To (2nd) value of the count attribute
        /// </summary>
        public int To { get; }

        private decimal _probFactor;

        /// <summary>
        /// Instanmtiate a new Count class
        /// </summary>
        /// <param name="countStr">The raw value of the count attribute, as it comes from the XML
        /// eg "1,3", "all", "", or null</param>
        public Count(string? countStr)
        {
            if (countStr == "all")
            {
                IsAll = true;
                return;
            }
            if (string.IsNullOrEmpty(countStr)) countStr = "1,1";
            var range = countStr.Split(',');
            From = Convert.ToInt32(range[0]);
            if (range.Count() > 1)
            {
                To = Convert.ToInt32(range[1]);
            }
            else
            {
                To = From;
            }
            if (From > To)
            {
                throw new ArgumentException($"Invalid Count of {countStr} - From must not be higher than To");
            }
            var entries = 0;
            var total = 0;
            for (int i = From; i <= To; i++)
            {
                entries++;
                total += i;
            }
            _probFactor = (decimal)total / entries;
        }

        /// <summary>
        /// Checks whether a value falls within the range of this Count
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True for is in range, False for not in range</returns>
        public bool IsInRange(int value)
        {
            if (IsAll) return true;
            return (value >= From && value <= To);
        }

        public decimal AdjustProbForCount(decimal prob)
        {
            if (IsAll) throw new Exception("Should not be calling with count of all, as it does not take into account force_prob");
            return prob * _probFactor;
        }

        /// <summary>
        /// When debugging or displaying in a UI, converts Count to a string representation
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            if (IsAll) return "all";
            if (From == To) return From.ToString();
            return $"{From},{To}";
        }

        public bool Equals(Count? other)
        {
            if (other == null) return false;
            return other.From == From && other.To == To && other.IsAll == IsAll;
        }
    }
}
