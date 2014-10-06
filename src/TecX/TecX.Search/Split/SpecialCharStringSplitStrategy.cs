namespace TecX.Search.Split
{
    using TecX.Search;

    public class SpecialCharStringSplitStrategy : RegexBasedStringSplitStrategy
    {
        public SpecialCharStringSplitStrategy()
            : base(Constants.RegularExpressions.Special)
        {
        }
    }
}