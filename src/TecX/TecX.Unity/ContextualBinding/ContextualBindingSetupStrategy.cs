using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingSetupStrategy : BuilderStrategy
    {
        private readonly IRequestHistory _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingSetupStrategy"/> class
        /// </summary>
        public ContextualBindingSetupStrategy(IRequestHistory history)
        {
            Guard.AssertNotNull(history, "history");

            _history = history;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            IRequest previous = _history.Count > 0 ? _history.Peek() : null;

            IRequest request = new Request(previous, _history.Count + 1, context);

            _history.Push(request);
        }
    }
}