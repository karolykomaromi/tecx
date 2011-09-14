using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.Unity.Collections
{
    public abstract class ResolverOverrideExtractor
    {
        public abstract ResolverOverride[] ExtractResolverOverrides(IBuilderContext context);
    }
}