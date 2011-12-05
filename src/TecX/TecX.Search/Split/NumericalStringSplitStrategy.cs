namespace TecX.Search.Split
{
    using TecX.Search;

    public class NumericalStringSplitStrategy : RegexBasedStringSplitStrategy
    {
        public NumericalStringSplitStrategy()
            : base(Constants.RegularExpressions.Numerical)
        {
        }
    }
}
