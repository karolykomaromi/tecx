using System.Collections.Generic;
using System.Windows.Ink;

using TecX.Common;

namespace TecX.Agile.Modules.Gestures.ViewModels
{
    public class GestureStrategyChain
    {
        private readonly List<GestureRecognitionStrategy> _strategies;

        public GestureStrategyChain()
        {
            _strategies = new List<GestureRecognitionStrategy>();
        }

        public void Add(GestureRecognitionStrategy strategy)
        {
            Guard.AssertNotNull(strategy, "strategy");

            _strategies.Add(strategy);
        }

        public void Process(ApplicationGesture gesture, StrokeCollection strokes)
        {
            Guard.AssertNotNull(strokes, "strokes");

            for (int i = 0; i < _strategies.Count; i++)
            {
                if (_strategies[i].Process(gesture, strokes))
                {
                    return;
                }
            }
        }
    }
}
