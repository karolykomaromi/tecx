namespace TecX.Unity.ContextualBinding
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ContextualParameterBindingPolicy : IContextualParameterBindingPolicy
    {
        private readonly List<ContextualResolverOverride> overrides;

        public ContextualParameterBindingPolicy()
        {
            this.overrides = new List<ContextualResolverOverride>();
        }

        public void Add(ContextualResolverOverride contextualResolverOverride)
        {
            Guard.AssertNotNull(contextualResolverOverride, "contextualResolverOverride");

            this.overrides.Add(contextualResolverOverride);
        }

        public void SetResolverOverrides(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");
            Guard.AssertNotNull(builderContext, "builderContext");

            foreach (ContextualResolverOverride contextualResolverOverride in this.overrides)
            {
                if (contextualResolverOverride.IsMatch(bindingContext, builderContext))
                {
                    contextualResolverOverride.SetResolverOverrides(builderContext);
                }
            }
        }
    }
}