using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            Type factoryType = typeof(TFactory);

            Guard.AssertCondition(factoryType.IsInterface, factoryType, "TFactory", "Cannot generate an implementation for a non-interface factory type.");

            ITypedFactoryConfiguration configuration = container.Configure<ITypedFactoryConfiguration>();

            configuration.RegisterFactory<TFactory>();

            return container;
        }
    }
}
