using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.Unity.Collections
{
    public class NullResolverOverrideExtractor : ResolverOverrideExtractor
    {
        public override ResolverOverride[] ExtractResolverOverrides(IBuilderContext context)
        {
            return new ResolverOverride[0];
        }
    }
}
