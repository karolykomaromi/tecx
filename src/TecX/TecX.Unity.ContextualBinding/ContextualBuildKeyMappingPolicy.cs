using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBuildKeyMappingPolicy : IBuildKeyMappingPolicy
    {
        #region Fields

        private readonly IBindingContext _bindingContext;
        private readonly List<ContextualBuildKeyMapping> _mappings;

        #endregion Fields

        #region Properties

        public IBuildKeyMappingPolicy LastChance { get; set; }

        #endregion Properties

        #region c'tor

        public ContextualBuildKeyMappingPolicy(IBindingContext bindingContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");

            _bindingContext = bindingContext;

            _mappings = new List<ContextualBuildKeyMapping>();
        }

        #endregion c'tor

        #region Implementation of IBuildKeyMappingPolicy

        public NamedTypeBuildKey Map(NamedTypeBuildKey buildKey, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(buildKey, "buildKey");
            Guard.AssertNotNull(builderContext, "builderContext");

            foreach (var mapping in _mappings)
            {
                if (mapping.Matches(_bindingContext, builderContext))
                {
                    return mapping.BuildKey;
                }
            }

            if (LastChance != null)
            {
                return LastChance.Map(buildKey, builderContext);
            }

            throw new ContextualBindingException("No contextual mapping that matches the current context was " +
                                                 "defined and no default mapping could be found.");
        }

        #endregion Implementation of IBuildKeyMappingPolicy

        #region Methods

        public void AddMapping(Predicate<IBindingContext, IBuilderContext> matches, Type mapTo, string uniqueMappingName)
        {
            //guards
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(mapTo, "mapTo");
            Guard.AssertNotNull(uniqueMappingName, "uniqueMappingName");

            _mappings.Add(new ContextualBuildKeyMapping(matches, mapTo, uniqueMappingName));
        }

        #endregion Methods
    }
}