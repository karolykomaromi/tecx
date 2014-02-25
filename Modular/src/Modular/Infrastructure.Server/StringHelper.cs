namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public static class StringHelper
    {
        public static string ToUpperCamelCase(string s)
        {
            Contract.Requires(!string.IsNullOrEmpty(s));

            string[] dotSeparated = s.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (dotSeparated.Length > 1)
            {
                return string.Join(".", dotSeparated.Select(ToUpperCamelCase));
            }

            string[] underscoreSeparated = s.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

            if (underscoreSeparated.Length > 1)
            {
                return string.Join(string.Empty, underscoreSeparated.Select(ToUpperCamelCase));
            }

            return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
        }

        public static string FirstCharacterToLowerInvariant(string s)
        {
            Contract.Requires(!string.IsNullOrEmpty(s));

            return s.Substring(0, 1).ToLowerInvariant() + s.Substring(1);
        }
    }
}