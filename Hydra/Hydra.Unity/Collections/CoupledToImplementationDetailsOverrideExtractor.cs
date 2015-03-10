namespace Hydra.Unity.Collections
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class CoupledToImplementationDetailsOverrideExtractor : ResolverOverrideExtractor
    {
        private static readonly FieldInfo ResolverOverridesField = typeof(BuilderContext).GetField(
            "resolverOverrides", BindingFlags.Instance | BindingFlags.NonPublic);

        public override ResolverOverride[] ExtractResolverOverrides(IBuilderContext context)
        {
            // this method is tightly coupled to the implementation of IBuilderContext. It assumes that the 
            // class BuilderContext is used and that a field of type CompositeResolverOverride named 'resolverOverrides'
            // exists in that class
            ResolverOverride[] resolverOverrides = new ResolverOverride[0];

            if (!(context is BuilderContext))
            {
                return resolverOverrides;
            }

            if (ResolverOverridesField == null)
            {
                return resolverOverrides;
            }

            var overrides = ResolverOverridesField.GetValue(context) as IEnumerable<ResolverOverride>;

            if (overrides != null)
            {
                resolverOverrides = overrides.ToArray();
            }

            return resolverOverrides;
        }
    }
}