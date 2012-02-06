﻿namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ContextualBuildKeyMappingPolicy : IBuildKeyMappingPolicy
    {
        private readonly IBindingContext bindingContext;

        private readonly List<ContextualBuildKeyMapping> mappings;

        private IBuildKeyMappingPolicy defaultMapping;

        public ContextualBuildKeyMappingPolicy(IBindingContext bindingContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");

            this.bindingContext = bindingContext;

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

            foreach (var mapping in this.mappings)
            {
                if (mapping.IsMatch(this.bindingContext, builderContext))
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

        public void AddMapping(Predicate<IBindingContext, IBuilderContext> predicate, Type mapTo, string uniqueMappingName)
        {
            Guard.AssertNotNull(predicate, "predicate");
            Guard.AssertNotNull(mapTo, "mapTo");
            Guard.AssertNotNull(uniqueMappingName, "uniqueMappingName");

            this.mappings.Add(new ContextualBuildKeyMapping(predicate, mapTo, uniqueMappingName));
        }
    }
}