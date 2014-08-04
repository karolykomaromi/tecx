namespace TecX.StringSimilarity
{
	public interface IStringSimilarityAlgorithm
	{
		double GetSimilarity(string s, string t);
	}
}