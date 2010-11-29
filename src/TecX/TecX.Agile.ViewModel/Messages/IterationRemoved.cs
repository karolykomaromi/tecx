using TecX.Common;

namespace TecX.Agile.ViewModel.Messages
{
    public class IterationRemoved : IDomainEvent
    {
        private readonly Iteration _iteration;
        private readonly IterationCollection _from;

        public IterationCollection From
        {
            get { return _from; }
        }

        public Iteration Iteration
        {
            get { return _iteration; }
        }

        public IterationRemoved(Iteration iteration, IterationCollection from)
        {
            Guard.AssertNotNull(iteration, "iteration");
            Guard.AssertNotNull(from, "to");

            _iteration = iteration;
            _from = from;
        }
    }
}