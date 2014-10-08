namespace TecX.StringSimilarity
{
    using System;
    using System.Collections.Generic;

    public class DamerauLevenshteinDistance : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            int distance = this.GetDamerauLevenshteinDistance(s, t);

            return distance == 0 ? 1 : 1 / (double)distance;
        }

        public int GetDamerauLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                {
                    return 0;
                }

                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int m = s.Length;
            int n = t.Length;
            int[,] h = new int[m + 2, n + 2];

            int inf = m + n;
            h[0, 0] = inf;

            for (int i = 0; i <= m; i++)
            {
                h[i + 1, 1] = i; h[i + 1, 0] = inf;
            }

            for (int j = 0; j <= n; j++)
            {
                h[1, j + 1] = j; h[0, j + 1] = inf;
            }

            SortedDictionary<char, int> sd = new SortedDictionary<char, int>();
            foreach (char letter in (s + t))
            {
                if (!sd.ContainsKey(letter))
                {
                    sd.Add(letter, 0);
                }
            }

            for (int i = 1; i <= m; i++)
            {
                int db = 0;
                for (int j = 1; j <= n; j++)
                {
                    int i1 = sd[t[j - 1]];
                    int j1 = db;

                    if (s[i - 1] == t[j - 1])
                    {
                        h[i + 1, j + 1] = h[i, j];
                        db = j;
                    }
                    else
                    {
                        h[i + 1, j + 1] = Math.Min(h[i, j], Math.Min(h[i + 1, j], h[i, j + 1])) + 1;
                    }

                    h[i + 1, j + 1] = Math.Min(h[i + 1, j + 1], h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[s[i - 1]] = i;
            }

            return h[m + 1, n + 1];
        }
    }
}