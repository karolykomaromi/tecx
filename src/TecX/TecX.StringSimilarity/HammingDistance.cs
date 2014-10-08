namespace TecX.StringSimilarity
{
    using System;

    public class HammingDistance : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            StringHelper.AssertSameLength(s, t);

            int distance = this.GetHammingDistance(s, t);

            return distance == 0 ? 1 : (double)1 / distance;
        }

        public int GetHammingDistance(string s, string t)
        {
            StringHelper.AssertSameLength(s, t);

            int differences = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (!string.Equals(s.Substring(i, 1), t.Substring(i, 1), StringComparison.Ordinal))
                {
                    differences++;
                }
            }

            return differences;
        }
    }
}