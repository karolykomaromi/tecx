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

        public void SetResolverOverrides(IRequest request)
        {
            Guard.AssertNotNull(request, "request");
            Guard.AssertNotNull(request.Build, "request.Build");

            foreach (ContextualResolverOverride contextualResolverOverride in this.overrides)
            {
                if (contextualResolverOverride.IsMatch(request))
                {
                    contextualResolverOverride.SetResolverOverrides(request.Build);
                }
            }
        }
    }
}