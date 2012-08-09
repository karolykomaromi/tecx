namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ContextualBuildKeyMappingPolicy : IBuildKeyMappingPolicy
    {
        private readonly IRequest request;

        private readonly List<ContextualBuildKeyMapping> mappings;

        private IBuildKeyMappingPolicy defaultMapping;

        public ContextualBuildKeyMappingPolicy(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            this.request = request;

            this.mappings = new List<ContextualBuildKeyMapping>();
        }

        public IBuildKeyMappingPolicy DefaultMapping
        {
            get
            {
                return this.defaultMapping;
            }

            set
            {
                Guard.AssertNotNull(value, "DefaultMapping");

                this.defaultMapping = value;
            }
        }

        public NamedTypeBuildKey Map(NamedTypeBuildKey buildKey, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(buildKey, "buildKey");
            Guard.AssertNotNull(builderContext, "builderContext");

            IBuilderContext before = this.request.Build;

            try
            {
                this.request.Build = builderContext;

                foreach (var mapping in this.mappings)
                {
                    if (mapping.IsMatch(this.request))
                    {
                        return mapping.BuildKey;
                    }
                }

                if (this.DefaultMapping != null)
                {
                    return this.DefaultMapping.Map(buildKey, builderContext);
                }

                throw new ContextualBindingException("No contextual mapping that matches the current context was " +
                                                     "defined and no default mapping could be found.");
            }
            finally
            {
                this.request.Build = before;
            }
        }

        public void AddMapping(Type mapTo, string uniqueMappingName, Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");
            Guard.AssertNotNull(mapTo, "mapTo");
            Guard.AssertNotNull(uniqueMappingName, "uniqueMappingName");

            this.mappings.Add(new ContextualBuildKeyMapping(predicate, mapTo, uniqueMappingName));
        }
    }
}