namespace TecX.Agile.Modules.Speech.Recognition
{
    using TecX.Common;

    public static class SpeechStrategyChainExtensions
    {
        public static SpeechStrategyChain Initialize(this SpeechStrategyChain chain)
        {
            Guard.AssertNotNull(chain, "chain");

            chain.Add(new AddStoryCardStrategy());

            return chain;
        }
    }
}