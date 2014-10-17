namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text;
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
    }
}