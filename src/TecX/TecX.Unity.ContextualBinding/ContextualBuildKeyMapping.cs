using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBuildKeyMapping
    {
        private readonly Predicate<IBindingContext, IBuilderContext> _isMatch;
        private readonly Type _mapTo;
        private readonly string _uniqueMappingName;

        public NamedTypeBuildKey BuildKey
        {
            get { return new NamedTypeBuildKey(_mapTo, _uniqueMappingName); }
        }

        public ContextualBuildKeyMapping(Predicate<IBindingContext, IBuilderContext> isMatch, Type mapTo, string uniqueMappingName)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotNull(mapTo, "mapTo");
            Guard.AssertNotEmpty(uniqueMappingName, "uniqueMappingName");

            _isMatch = isMatch;
            _mapTo = mapTo;
            _uniqueMappingName = uniqueMappingName;
        }

        public bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");
            Guard.AssertNotNull(builderContext, "builderContext");

            return _isMatch(bindingContext, builderContext);
        }
    }
}