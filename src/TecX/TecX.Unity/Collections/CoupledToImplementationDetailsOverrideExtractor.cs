using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.Unity.Collections
{
    public class CoupledToImplementationDetailsOverrideExtractor : ResolverOverrideExtractor
    {
        private static class Constants
        {
            /// <summary>resolverOverrides</summary>
            public const string ResolverOverrides = "resolverOverrides";
        }

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

            FieldInfo field = typeof(BuilderContext).GetField(Constants.ResolverOverrides, BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
            {
                return resolverOverrides;
            }

            var overrides = field.GetValue(context) as IEnumerable<ResolverOverride>;

            if (overrides != null)
            {
                resolverOverrides = overrides.ToArray();
            }

            return resolverOverrides;
        }
    }
}