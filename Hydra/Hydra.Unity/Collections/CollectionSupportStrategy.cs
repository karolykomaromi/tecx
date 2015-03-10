namespace Hydra.Unity.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class CollectionSupportStrategy : BuilderStrategy
    {
        private static readonly MethodInfo GenericResolveCollectionMethod = 
            typeof(CollectionSupportStrategy).GetMethod("ResolveCollection", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

        private delegate object CollectionResolver(IBuilderContext context);

        public override void PreBuildUp(IBuilderContext context)
        {
            Contract.Requires(context != null);

            Type typeToBuild = context.BuildKey.Type;

            if (typeToBuild.IsGenericType)
            {
                Type openGeneric = typeToBuild.GetGenericTypeDefinition();

                if (openGeneric == typeof(IEnumerable<>) ||
                    openGeneric == typeof(ICollection<>) ||
                    openGeneric == typeof(IList<>))
                {
                    Type elementType = typeToBuild.GetGenericArguments()[0];

                    MethodInfo resolverMethod = CollectionSupportStrategy.GenericResolveCollectionMethod.MakeGenericMethod(elementType);

                    CollectionResolver resolver = (CollectionResolver)Delegate.CreateDelegate(typeof(CollectionResolver), resolverMethod);

                    context.Existing = resolver(context);
                    context.BuildComplete = true;
                }
            }
        }

        private static object ResolveCollection<T>(IBuilderContext context)
        {
            IUnityContainer container = context.NewBuildUp<IUnityContainer>();

            var extractor = context.NewBuildUp<ResolverOverrideExtractor>();

            var resolverOverrides = extractor.ExtractResolverOverrides(context);

            List<T> results = new List<T>(container.ResolveAll<T>(resolverOverrides));

            return results;
        }
    }
}
