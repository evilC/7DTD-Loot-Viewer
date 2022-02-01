namespace ConfigParsers.Common
{
    public static class Parsers
    {
        public static Range? ParseRange(string rawRange)
        {
            if (string.IsNullOrEmpty(rawRange)) return null;
            if (rawRange == "all")
            {
                // ToDo: Not sure how to deal with this scenario - for now I will just use a magic number
                return new Range(0, int.MaxValue);
            }
            var range = rawRange.Split(',');
            var rangeLow = Convert.ToInt32(range[0]);
            int rangeHigh;
            if (range.Count() > 1)
            {
                rangeHigh = Convert.ToInt32(range[1]);
            }
            else
            {
                rangeHigh = rangeLow;
            }
            return new Range(rangeHigh, rangeLow);
        }
    }
}
