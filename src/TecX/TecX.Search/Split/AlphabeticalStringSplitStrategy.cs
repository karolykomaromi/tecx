namespace TecX.Search.Split
{
    using TecX.Search;

    public class AlphabeticalStringSplitStrategy : RegexBasedStringSplitStrategy
    {
        public AlphabeticalStringSplitStrategy()
            : base(Constants.RegularExpressions.Alphabetical)
        {
        }
    }
}
