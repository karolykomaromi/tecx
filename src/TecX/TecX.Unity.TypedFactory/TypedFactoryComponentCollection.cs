namespace TecX.Unity.TypedFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypedFactoryComponentCollection<TItem> : TypedFactoryComponent
            where TItem : class
    {
        public TypedFactoryComponentCollection(Type typeToBuild, ResolverOverrides additionalArguments)
            : base(typeToBuild, additionalArguments)
        {
        }

        public override object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            IEnumerable<TItem> resolved = container.ResolveAll<TItem>(AdditionalArguments.ToArray());

            if (TypeToBuild.IsAssignableFrom(typeof(IEnumerable<TItem>)))
            {
                return resolved;
            }

            if (TypeToBuild.IsArray)
            {
                return resolved.ToArray();
            }

            return resolved.ToList();
        }
    }
}