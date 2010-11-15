using TecX.Common;

namespace TecX.Agile.ViewModel.Messages
{
    public class IterationAdded : IMessage
    {
        private readonly Iteration _iteration;
        private readonly IterationCollection _to;

        public IterationCollection To
        {
            get { return _to; }
        }

        public Iteration Iteration
        {
            get { return _iteration; }
        }

        public IterationAdded(Iteration iteration, IterationCollection to)
        {
            Guard.AssertNotNull(iteration, "iteration");
            Guard.AssertNotNull(to, "to");

            _iteration = iteration;
            _to = to;
        }
    }
}