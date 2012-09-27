namespace TecX.StringSimilarity
{
    using System;
    using System.Linq;

    public class DicesCoefficient : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            return this.GetDicesCoefficient(s, t);
        }

        public double GetDicesCoefficient(string s, string t)
        {
            s = s ?? string.Empty;
            t = t ?? string.Empty;

            string[] bigramsX = StringHelper.GetBigrams(s);
            string[] bigramsY = StringHelper.GetBigrams(t);

            string[] intersection = bigramsX.Intersect(bigramsY, StringComparer.OrdinalIgnoreCase).ToArray();

            double nx = bigramsX.Length;
            double ny = bigramsY.Length;
            double nt = intersection.Length;

            double similarity = 2 * nt / (nx + ny);

            return Math.Round(similarity, 2, MidpointRounding.ToEven);
        }
    }
}