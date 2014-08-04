namespace TecX.StringSimilarity
{
    using System;
    using System.Linq;

    public class JaccardIndex : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            return this.GetJaccardIndex(s, t);
        }

        public double GetJaccardDistance(string s, string t)
        {
            return 1 - this.GetJaccardIndex(s, t);
        }

        public double GetJaccardIndex(string s, string t)
        {
            s = s ?? string.Empty;
            t = t ?? string.Empty;

            string[] bigramsS = StringHelper.GetBigrams(s);
            string[] bigramsT = StringHelper.GetBigrams(t);

            string[] intersection = bigramsS.Intersect(bigramsT, StringComparer.Ordinal).ToArray();

            string[] union = bigramsS.Union(bigramsT, StringComparer.Ordinal).ToArray();

            double coefficient = (double)intersection.Length / (double)union.Length;

            return coefficient;
        }
    }
}