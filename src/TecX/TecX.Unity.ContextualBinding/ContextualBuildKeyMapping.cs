namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ContextualBuildKeyMapping
    {
        private readonly Predicate<IBindingContext, IBuilderContext> isMatch;
        private readonly Type mapTo;
        private readonly string uniqueMappingName;
        
        public ContextualBuildKeyMapping(Predicate<IBindingContext, IBuilderContext> isMatch, Type mapTo, string uniqueMappingName)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotNull(mapTo, "mapTo");
            Guard.AssertNotEmpty(uniqueMappingName, "uniqueMappingName");

            this.isMatch = isMatch;
            this.mapTo = mapTo;
            this.uniqueMappingName = uniqueMappingName;
        }

        public NamedTypeBuildKey BuildKey
        {
            get { return new NamedTypeBuildKey(this.mapTo, this.uniqueMappingName); }
        }

        public bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");
            Guard.AssertNotNull(builderContext, "builderContext");

            return this.isMatch(bindingContext, builderContext);
        }
    }
}