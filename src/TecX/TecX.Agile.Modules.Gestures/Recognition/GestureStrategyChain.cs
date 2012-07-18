namespace TecX.Agile.Modules.Gestures.Recognition
{
    using System.Collections.Generic;

    using TecX.Common;

    public class GestureStrategyChain
    {
        private readonly List<GestureRecognitionStrategy> strategies;

        public GestureStrategyChain()
        {
            this.strategies = new List<GestureRecognitionStrategy>();
        }

        public void Add(GestureRecognitionStrategy strategy)
        {
            Guard.AssertNotNull(strategy, "strategy");

            this.strategies.Add(strategy);
        }

        public void Process(GestureRecognitionContext context)
        {
            Guard.AssertNotNull(context, "context");

            for (int i = 0; i < this.strategies.Count; i++)
            {
                this.strategies[i].Process(context);

                if (context.RecognitionCompleted)
                {
                    return;
                }
            }
        }
    }
}
