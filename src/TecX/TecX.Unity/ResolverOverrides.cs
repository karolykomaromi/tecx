namespace TecX.Unity
{
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ResolverOverrides
    {
        private readonly List<ResolverOverride> overrides;

        public ResolverOverrides()
        {
            this.overrides = new List<ResolverOverride>();
        }

        public static implicit operator ResolverOverride[](ResolverOverrides overrides)
        {
            Guard.AssertNotNull(overrides, "overrides");

            return overrides.overrides.ToArray();
        }

        public void Add(ResolverOverride @override)
        {
            Guard.AssertNotNull(@override, "override");

            this.overrides.Add(@override);
        }
    }
}