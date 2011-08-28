using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Collections
{
    public class CollectionResolutionStrategy : BuilderStrategy
    {
        private delegate object CollectionResolver(IBuilderContext context);

        private static readonly MethodInfo genericResolveCollectionMethod = typeof(CollectionResolutionStrategy)
                .GetMethod("ResolveCollection", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            Type typeToBuild = context.BuildKey.Type;

            if (typeToBuild.IsGenericType)
            {
                Type openGeneric = typeToBuild.GetGenericTypeDefinition();

                if (openGeneric == typeof(IEnumerable<>) ||
                    openGeneric == typeof(ICollection<>) ||
                    openGeneric == typeof(IList<>))
                {
                    Type elementType = typeToBuild.GetGenericArguments()[0];

                    MethodInfo resolverMethod = genericResolveCollectionMethod.MakeGenericMethod(elementType);

                    CollectionResolver resolver = (CollectionResolver)Delegate.CreateDelegate(typeof(CollectionResolver), resolverMethod);

                    context.Existing = resolver(context);
                    context.BuildComplete = true;
                }
            }
        }

        private static object ResolveCollection<T>(IBuilderContext context)
        {
            IUnityContainer container = context.NewBuildUp<IUnityContainer>();

            var resolverOverrides = ExtractResolverOverrides(context);

            List<T> results = new List<T>(container.ResolveAll<T>(resolverOverrides));

            return results;
        }

        private static ResolverOverride[] ExtractResolverOverrides(IBuilderContext context)
        {
            //this method is tightly coupled to the implementation of IBuilderContext. It assumes that the 
            //class BuilderContext is used and that a field of type CompositeResolverOverride named resolverOverrides
            //exists in that class
            ResolverOverride[] ro = new ResolverOverride[0];

            if (context is BuilderContext)
            {
                FieldInfo field = typeof (BuilderContext).GetField("resolverOverrides",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (field != null)
                {
                    var overrides = field.GetValue(context) as IEnumerable<ResolverOverride>;

                    if (overrides != null)
                    {
                        ro = overrides.ToArray();
                    }
                }
            }

            return ro;
        }
    }
}
