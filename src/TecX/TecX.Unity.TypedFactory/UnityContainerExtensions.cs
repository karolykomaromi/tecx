namespace TecX.Unity.TypedFactory
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container)
            where TFactory : class
        {
            Guard.AssertNotNull(container, "container");

            return RegisterFactory(container, typeof(TFactory), new DefaultTypedFactoryComponentSelector());
        }

        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(factoryType, "factoryType");

            return RegisterFactory(container, factoryType, new DefaultTypedFactoryComponentSelector());
        }

        public static IUnityContainer RegisterFactory<TFactory>(
            this IUnityContainer container, 
            ITypedFactoryComponentSelector selector)
            where TFactory : class
        {
            Guard.AssertNotNull(container, "container");

            return RegisterFactory(container, typeof(TFactory), selector);
        }

        public static IUnityContainer RegisterFactory(
            this IUnityContainer container,
            Type factoryType,
            ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(factoryType, "factoryType");
            Guard.AssertNotNull(selector, "selector");

            Guard.AssertCondition(
                factoryType.IsInterface,
                factoryType,
                "TFactory",
                "Cannot generate an implementation for a non-interface factory type.");

            ITypedFactoryConfiguration configuration = container.Configure<ITypedFactoryConfiguration>();

            configuration.RegisterFactory(factoryType, selector);

            return container;
        }
    }
}
