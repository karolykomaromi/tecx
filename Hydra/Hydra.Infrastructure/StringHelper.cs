namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    public static class StringHelper
    {
        private static readonly Regex CamelHumps = new Regex("(?<=[a-z])([A-Z])", RegexOptions.Compiled);

        public static string SplitCamelCase(string s)
        {
            Contract.Requires(s != null);

            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string splitCamelCase = StringHelper.CamelHumps.Replace(s, " $1").Trim();

            return splitCamelCase;
        }
    }
}