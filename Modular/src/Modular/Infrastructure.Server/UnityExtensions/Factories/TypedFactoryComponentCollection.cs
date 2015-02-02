namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.Practices.Unity;

    public class TypedFactoryComponentCollection<TItem> : TypedFactoryComponent
        where TItem : class
    {
        public TypedFactoryComponentCollection(Type typeToBuild, params ResolverOverride[] additionalArguments)
            : base(typeToBuild, additionalArguments)
        {
        }

        public override object Resolve(IUnityContainer container)
        {
            Contract.Requires(container != null);

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