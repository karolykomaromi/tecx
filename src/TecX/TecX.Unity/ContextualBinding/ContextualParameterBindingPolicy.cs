namespace TecX.Unity.ContextualBinding
{
    using System.Collections.Generic;

    using TecX.Common;
    using TecX.Unity.Tracking;

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
            Guard.AssertNotNull(request.BuilderContext, "request.BuilderContext");

            foreach (ContextualResolverOverride contextualResolverOverride in this.overrides)
            {
                if (contextualResolverOverride.IsMatch(request))
                {
                    contextualResolverOverride.SetResolverOverrides(request.BuilderContext);
                }
            }
        }
    }
}