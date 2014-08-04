namespace TecX.Search.Split
{
    using TecX.Search;

    public class IpAddressStringSplitStrategy : RegexBasedStringSplitStrategy
    {
        public IpAddressStringSplitStrategy()
            : base(Constants.RegularExpressions.IpAddress)
        {
        }
    }
}
