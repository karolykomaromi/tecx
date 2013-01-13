namespace TecX.StringSimilarity
{
    using System;
    using System.Collections.Generic;

    public static class StringHelper
    {
        public static string[] GetBigrams(string s)
        {
            if (String.IsNullOrEmpty(s) || s.Length < 2)
            {
                return new string[0];
            }

            List<string> bigrams = new List<string>();

            for (int i = 0; i < s.Length - 1; i++)
            {
                bigrams.Add(s.Substring(i, 2));
            }

            return bigrams.ToArray();
        }

        public static void AssertSameLength(string s, string t)
        {
            if (s.Length != t.Length)
            {
                throw new ArgumentOutOfRangeException(String.Empty,
                    "Hamming distance is only defined for strings of same length " +
                        "but input strings have different length.");
            }
        }
    }
}