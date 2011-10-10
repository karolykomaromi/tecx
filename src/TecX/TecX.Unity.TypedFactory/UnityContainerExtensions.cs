using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container)
            where TFactory : class
        {
            Guard.AssertNotNull(container, "container");

            return RegisterFactory<TFactory>(container, new DefaultTypedFactoryComponentSelector());
        }

        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container, 
            ITypedFactoryComponentSelector selector)
            where TFactory : class
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(selector, "selector");

            Type factoryType = typeof(TFactory);

            Guard.AssertCondition(factoryType.IsInterface, 
                factoryType, 
                "TFactory", 
                "Cannot generate an implementation for a non-interface factory type.");

            ITypedFactoryConfiguration configuration = container.Configure<ITypedFactoryConfiguration>();

            configuration.RegisterFactory<TFactory>(selector);

            return container;
        }
    }
}
