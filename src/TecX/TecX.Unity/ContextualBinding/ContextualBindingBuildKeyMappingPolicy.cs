using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingBuildKeyMappingPolicy : IBuildKeyMappingPolicy
    {
        private readonly IRequestHistory _history;
        private readonly Func<IRequest, bool> _shouldResolve;
        private readonly Type _to;

        public ContextualBindingBuildKeyMappingPolicy Next { get; set; }
        public IBuildKeyMappingPolicy LastChance { get; set; }

        public ContextualBindingBuildKeyMappingPolicy(IRequestHistory history,
            Func<IRequest, bool> shouldResolve, Type to)
        {
            Guard.AssertNotNull(history, "history");
            Guard.AssertNotNull(shouldResolve, "shouldResolve");
            Guard.AssertNotNull(to, "to");

            _history = history;
            _shouldResolve = shouldResolve;
            _to = to;
        }

        public bool ShouldResolve(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            return _shouldResolve(request);
        }

        #region Implementation of IBuildKeyMappingPolicy

        /// <summary>
        /// Maps the build key.
        /// </summary>
        /// <param name="buildKey">The build key to map.</param>
        /// <param name="context">Current build context. Used for contextual information
        /// if writing a more sophisticated mapping. This parameter can be null
        /// (called when getting container registrations).</param>
        /// <returns>The new build key.</returns>
        public NamedTypeBuildKey Map(NamedTypeBuildKey buildKey, IBuilderContext context)
        {
            if (ShouldResolve(_history.Peek().Previous))
            {
                return new NamedTypeBuildKey(_to);
            }

            if (Next == null &&
                LastChance != null)
            {
                return LastChance.Map(buildKey, context);
            }

            var current = Next;
            while (current != null)
            {
                if (current.ShouldResolve(_history.Peek().Previous))
                {
                    return new NamedTypeBuildKey(current._to);
                }

                if (current.Next == null && current.LastChance != null)
                    return current.LastChance.Map(buildKey, context);

                current = current.Next;
            }

            throw new ContextualBindingException("No contextual mapping that matches the current context was " +
                "defined and no default mapping could be found.");
        }

        #endregion
    }
}