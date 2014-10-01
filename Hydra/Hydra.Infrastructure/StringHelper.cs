namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    public static class StringHelper
    {
        public static string SplitCamelCase(string s)
        {
            Contract.Requires(s != null);

            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string splitCamelCase = Regex.Replace(s, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled).Trim();

            return splitCamelCase;
        }
    }
}