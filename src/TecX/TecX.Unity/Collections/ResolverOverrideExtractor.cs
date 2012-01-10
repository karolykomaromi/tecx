namespace TecX.Unity.Collections
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public abstract class ResolverOverrideExtractor
    {
        public abstract ResolverOverride[] ExtractResolverOverrides(IBuilderContext context);
    }
}