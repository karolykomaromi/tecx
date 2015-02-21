namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringHelper
    {
        private static readonly Regex CamelHumps = new Regex("(?<=[a-z])([A-Z])", RegexOptions.Compiled);

        private static readonly Regex NoCharactersNumbersAndUnderscores = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        private static readonly Regex MultipleUnderscores = new Regex("_{2,}", RegexOptions.Compiled);

        private static readonly Regex TrailingUnderscores = new Regex("_+$", RegexOptions.Compiled);

        public static string SplitCamelCase(string s)
        {
            Contract.Requires(s != null);

            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string splitCamelCase = CamelHumps.Replace(s, " $1").Trim();

            return splitCamelCase;
        }

        public static void SaveToFile(this string s, string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(s));
            Contract.Requires(!string.IsNullOrWhiteSpace(path));

            using (Stream stream = File.Create(path))
            {
                using (TextWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(s);
                    writer.Flush();
                }
            }
        }

        public static string CapitalizeFirstLetter(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            if (s.Length == 1)
            {
                return s.ToUpperInvariant();
            }

            return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
        }
    }
}