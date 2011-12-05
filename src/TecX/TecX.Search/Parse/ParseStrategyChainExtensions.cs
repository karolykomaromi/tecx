namespace TecX.Search.Parse
{
    using TecX.Common;

    public static class ParseStrategyChainExtensions
    {
        public static ParseStrategyChain Initialize(this ParseStrategyChain chain)
        {
            Guard.AssertNotNull(chain, "chain");

            chain.AddStrategy(new DateTimeParseStrategy());
            chain.AddStrategy(new Int32ParseStrategy());
            chain.AddStrategy(new Int64ParseStrategy());
            chain.AddStrategy(new FloatParseStrategy());
            chain.AddStrategy(new DoubleParseStrategy());
            chain.AddStrategy(new ForwardStringParseStrategy());

            return chain;
        }
    }
}