namespace TecX.Search.Data.EF
{
    using System.Text;

    public static class SqlServerHelper
    {
        public static string AdjustWildcards(string pattern)
        {
            StringBuilder sb = new StringBuilder(pattern);

            // replace windows wildcards with
            // sql wildcards
            sb.Replace("*", "%");
            sb.Replace("?", "_");

            // if the search term does not end with a wildcard add it
            if (sb[sb.Length - 1] != '%')
            {
                sb.Append("%");
            }

            pattern = sb.ToString();

            return pattern;
        }
    }
}