namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Ink;

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

        public void Process(ApplicationGesture gesture, StrokeCollection strokes)
        {
            Guard.AssertNotNull(strokes, "strokes");

            for (int i = 0; i < this.strategies.Count; i++)
            {
                if (this.strategies[i].Process(gesture, strokes))
                {
                    return;
                }
            }
        }
    }
}
