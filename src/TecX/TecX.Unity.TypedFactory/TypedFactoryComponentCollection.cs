using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryComponentCollection<TItem> : TypedFactoryComponent
            where TItem : class
    {
        public TypedFactoryComponentCollection(Type typeToBuild, ParameterOverrides additionalArguments)
            : base(typeToBuild, additionalArguments)
        {
        }

        public override object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            IEnumerable<TItem> resolved = container.ResolveAll<TItem>(AdditionalArguments);

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