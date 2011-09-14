﻿using System;
using System.Collections.Generic;
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

            var extractor = context.NewBuildUp<ResolverOverrideExtractor>();

            var resolverOverrides = extractor.ExtractResolverOverrides(context);

            List<T> results = new List<T>(container.ResolveAll<T>(resolverOverrides));

            return results;
        }
    }
}
