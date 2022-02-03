namespace ConfigParsers.Loot.Data
{
    /// <summary>
    /// Xml Count attributes are converted to this class, for easy processing
    /// </summary>
    public class Count
    {
        /// <summary>
        /// True if a count attribute was specified, otherwise false.
        /// If true, IsAll or From and To must be set
        /// </summary>
        public bool IsSet { get; } = false;

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

        public Count(string? rawRange)
        {
            if (string.IsNullOrEmpty(rawRange)) return;
            IsSet = true;
            if (rawRange == "all")
            {
                IsAll = true;
                return;
            }
            var range = rawRange.Split(',');
            From = Convert.ToInt32(range[0]);
            int rangeHigh;
            if (range.Count() > 1)
            {
                To = Convert.ToInt32(range[1]);
            }
            else
            {
                To = From;
            }
        }

        /// <summary>
        /// When debugging or displaying in a UI, converts Count to a string representation
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            if (!IsSet) return "<Not Set>";
            if (IsAll) return "all";
            if (From == To) return From.ToString();
            return $"{From},{To}";
        }
    }
}
