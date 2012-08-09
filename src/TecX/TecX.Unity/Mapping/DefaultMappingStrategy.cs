namespace TecX.Unity.Mapping
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class DefaultMappingStrategy : BuilderStrategy
    {
        private readonly IDictionary<Type, NamedTypeBuildKey> mappings;

        public DefaultMappingStrategy(IDictionary<Type, NamedTypeBuildKey> mappings)
        {
            Guard.AssertNotNull(mappings, "mappings");

            this.mappings = mappings;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");
            Guard.AssertNotNull(context.BuildKey.Type, "context.BuildKey.Type");

            if (context.BuildKey.Type.IsInterface || context.BuildKey.Type.IsAbstract)
            {
                NamedTypeBuildKey buildKey;
                if (this.mappings.TryGetValue(context.BuildKey.Type, out buildKey))
                {
                    context.BuildKey = buildKey;
                }
            }
        }
    }
}