namespace TecX.Search.Split
{
    using TecX.Common;

    public static class StringSplitStrategyChainExtensions
    {
        public static StringSplitStrategyChain Initialize(this StringSplitStrategyChain chain)
        {
            Guard.AssertNotNull(chain, "chain");

            chain.AddStrategy(new DateTimeStringSplitStrategy());
            chain.AddStrategy(new WhitespaceStringSplitStrategy());

            return chain;
        }
    }
}