namespace TecX.Unity.Collections
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class NullResolverOverrideExtractor : ResolverOverrideExtractor
    {
        public override ResolverOverride[] ExtractResolverOverrides(IBuilderContext context)
        {
            return new ResolverOverride[0];
        }
    }
}
