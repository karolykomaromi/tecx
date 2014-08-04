namespace TecX.Agile.Modules.Gestures.Recognition
{
    using TecX.Common;

    public static class GestureStrategyChainExtensions
    {
        public static GestureStrategyChain Initialize(this GestureStrategyChain chain)
        {
            Guard.AssertNotNull(chain, "chain");

            chain.Add(new AddStoryCardStrategy());

            return chain;
        }
    }
}
