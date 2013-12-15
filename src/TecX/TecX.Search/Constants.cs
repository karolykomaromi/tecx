namespace TecX.Search
{
    using System.Text.RegularExpressions;

    public static class Constants
    {
        public const string Blank = " ";

        public static class RegularExpressions
        {
            /// <summary>
            /// ([0-9]{1,3}\.){3}[0-9]{1,3}
            /// </summary>
            public static readonly Regex IpAddress = new Regex(@"([0-9]{1,3}\.){3}[0-9]{1,3}", RegexOptions.CultureInvariant);

            public static readonly Regex Alphabetical = new Regex("[A-Z]*", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            /// <summary>
            /// \s*\(\s*\)\s*
            /// </summary>
            public static readonly Regex Braces = new Regex(@"\s*\(\s*\)\s*", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            /// <summary>
            /// [0-9]+
            /// </summary>
            public static readonly Regex Numerical = new Regex(@"[0-9]+", RegexOptions.CultureInvariant);

            /// <summary>
            /// n times: an arbitrary number of characters that are neither pipes nor whitespaces followed by a pipe (with or without leading or trailing whitespaces)
            /// this sequence is terminated again by a sequence of non-whitespace and non-pipe characters of arbitrary length followed by one or more whitespaces.
            /// </summary>
            /// <remarks>(([^\|^\s]+\s*\|)+\s*[^\|^\s]+)</remarks>
            public static readonly Regex OrClause = new Regex(@"(([^\|^\s]+\s*\|)+\s*[^\|^\s]+)", RegexOptions.CultureInvariant);

            /// <summary>
            /// [^A-Z0-9\s]+
            /// </summary>
            public static readonly Regex Special = new Regex(@"[^A-Z0-9\s]+", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        
            /// <summary>
            /// [A-Z0-9]{5}
            /// </summary>
            public static readonly Regex MemberId = new Regex(@"[A-Z0-9]{5}", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }
    }
}
