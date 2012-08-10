namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Unity.Tracking;

    public class ContextualBuildKeyMapping
    {
        private readonly Predicate<IRequest> isMatch;
        private readonly Type mapTo;
        private readonly string uniqueMappingName;
        
        public ContextualBuildKeyMapping(Predicate<IRequest> isMatch, Type mapTo, string uniqueMappingName)
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

        public bool IsMatch(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            return this.isMatch(request);
        }
    }
}