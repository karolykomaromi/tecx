namespace Hydra.Unity.Collections
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public abstract class ResolverOverrideExtractor
    {
        public static readonly ResolverOverrideExtractor Null = new NullResolverOverrideExtractor();

        public abstract ResolverOverride[] ExtractResolverOverrides(IBuilderContext context);

        private class NullResolverOverrideExtractor : ResolverOverrideExtractor
        {
            public override ResolverOverride[] ExtractResolverOverrides(IBuilderContext context)
            {
                return new ResolverOverride[0];
            }
        }
    }
}