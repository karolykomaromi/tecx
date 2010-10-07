using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingPostInitStrategy : BuilderStrategy
    {
        private readonly IRequestHistory _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingSetupStrategy"/> class
        /// </summary>
        public ContextualBindingPostInitStrategy(IRequestHistory history)
        {
            Guard.AssertNotNull(history, "history");

            _history = history;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            Type typeThatWasBuilt = context.OriginalBuildKey.Type;

            if (_history.Count > 0 &&
                typeThatWasBuilt == _history.Peek().Context.BuildKey.Type)
            {
                _history.Pop();
            }
        }
    }
}