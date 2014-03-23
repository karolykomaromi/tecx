namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypedFactoryComponentCollection<TItem> : TypedFactoryComponent
            where TItem : class
    {
        public TypedFactoryComponentCollection(Type typeToBuild, ResolverOverride[] additionalArguments)
            : base(typeToBuild, additionalArguments)
        {
        }

        public override object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            IEnumerable<TItem> resolved = container.ResolveAll<TItem>(this.AdditionalArguments);

            if (this.TypeToBuild.IsAssignableFrom(typeof(IEnumerable<TItem>)))
            {
                return resolved;
            }

            if (this.TypeToBuild.IsArray)
            {
                return resolved.ToArray();
            }

            return resolved.ToList();
        }
    }
}