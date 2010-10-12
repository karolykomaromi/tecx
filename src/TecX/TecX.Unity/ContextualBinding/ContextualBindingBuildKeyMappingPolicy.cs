using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingBuildKeyMappingPolicy : IBuildKeyMappingPolicy
    {
        private readonly IRequestHistory _history;
        private readonly List<ContextualBuildKeyMapping> _mappings;

        public IBuildKeyMappingPolicy LastChance { get; set; }

        public ContextualBindingBuildKeyMappingPolicy(IRequestHistory history)
        {
            Guard.AssertNotNull(history, "history");

            _history = history;
            _mappings = new List<ContextualBuildKeyMapping>();
        }

        #region Implementation of IBuildKeyMappingPolicy

        /// <summary>
        /// Checks wether there is a matching buildkey mapping override registered and uses the first match,
        /// if any. If no override fits it checks wether there is a last chance default mapping present.
        /// Throws exception if no matches and no default mapping are found.
        /// </summary>
        /// <exception cref="ContextualBindingException">If no contextual mapping fits and no default mapping
        /// registered</exception>
        public NamedTypeBuildKey Map(NamedTypeBuildKey buildKey, IBuilderContext context)
        {
            foreach (var mapping in _mappings)
            {
                if (mapping.Matches(_history.Current().Parent))
                {
                    return mapping.MapTo;
                }
            }

            if (LastChance != null)
            {
                return LastChance.Map(buildKey, context);
            }

            throw new ContextualBindingException("No contextual mapping that matches the current context was " +
                                                 "defined and no default mapping could be found.");
        }

        #endregion

        public void AddMapping(Func<IRequest, bool> matches, Type mapTo)
        {
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(mapTo, "mapTo");

            _mappings.Add(new ContextualBuildKeyMapping(matches, mapTo));
        }
    }
}