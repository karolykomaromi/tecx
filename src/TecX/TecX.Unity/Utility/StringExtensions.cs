namespace TecX.Unity.Utility
{
    using System;
    using System.Globalization;

    using TecX.Common;

    public static class StringExtensions
    {
        public static string ToUpper(this string s, int startIndex, int length)
        {
            return ToUpper(s, startIndex, length, CultureInfo.CurrentCulture);
        }

        public static string ToUpper(this string s, int startIndex)
        {
            return ToUpper(s, startIndex, string.IsNullOrEmpty(s) ? 0 : s.Length);
        }

        public static string ToUpper(this string s, int startIndex, int length, CultureInfo culture)
        {
            Guard.AssertNotNull(culture, "culture");
            Guard.AssertInRange(startIndex, "startIndex", 0, int.MaxValue);

            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (length > s.Length)
            {
                length = s.Length;
            }

            if (s.Length < startIndex + length)
            {
                length = s.Length - startIndex;
            }

            string prefix = s.Substring(0, startIndex);

            string postfix = s.Substring(startIndex + length);

            string upperPart = s.Substring(startIndex, length).ToUpper(culture);

            return prefix + upperPart + postfix;
        }

        public static bool Contains(this string s, string pattern, StringComparison comparison)
        {
            Guard.AssertNotNull(s, "s");
            Guard.AssertNotNull(pattern, "pattern");

            return s.IndexOf(pattern, comparison) != -1;
        }
    }
}