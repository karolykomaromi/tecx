using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class RequestTrackerStrategy : BuilderStrategy
    {
        private readonly IRequestHistory _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTrackerStrategy"/> class
        /// </summary>
        public RequestTrackerStrategy(IRequestHistory history)
        {
            Guard.AssertNotNull(history, "history");

            _history = history;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            IRequest parent = _history.Current();

            IRequest request = new Request(parent, _history.Count + 1, context);

            _history.Append(request);
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            Type typeThatWasBuilt = context.OriginalBuildKey.Type;

            if (_history.Current() != null &&
                typeThatWasBuilt == _history.Current().TypeToBuild)
            {
                _history.RemoveCurrent();
            }
        }
    }
}