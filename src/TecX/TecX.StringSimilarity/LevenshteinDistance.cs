namespace TecX.StringSimilarity
{
    using System.Linq;

    public class LevenshteinDistance : IStringSimilarityAlgorithm
    {
        public double GetSimilarity(string s, string t)
        {
            int distance = this.GetLevenshteinDistance(s, t);

            return distance == 0 ? 1 : (double)1 / distance;
        }

        public int GetLevenshteinDistance(string s, string t)
        {
            s = s ?? string.Empty;
            t = t ?? string.Empty;

            int len_s = s.Length;
            int len_t = t.Length;

            int cost = 0;

            if (len_s == 0)
            {
                return len_t;
            }

            if (len_t == 0)
            {
                return len_s;
            }

            if (s[0] != t[0])
            {
                cost = 1;
            }

            return new[]
			       {
				       this.GetLevenshteinDistance(s.Substring(1), t) + 1, 
							 this.GetLevenshteinDistance(s, t.Substring(1)) + 1, 
							 this.GetLevenshteinDistance(s.Substring(1), t.Substring(1)) + cost
			       }.Min();
        }
    }
}