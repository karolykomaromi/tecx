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
            return RegisterFactory(container, typeof(TFactory));
        }

        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container, string name)
            where TFactory : class
        {
            return RegisterFactory(container, typeof(TFactory), name);
        }

        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container, ITypedFactoryComponentSelector selector)
            where TFactory : class
        {
            return RegisterFactory(container, typeof(TFactory), selector);
        }

        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container, string name, ITypedFactoryComponentSelector selector)
            where TFactory : class
        {
            return RegisterFactory(container, typeof(TFactory), name, selector);
        }

        public static IUnityContainer RegisterFactory<TFactory>(this IUnityContainer container, string name, ITypedFactoryComponentSelector selector, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
            where TFactory : class
        {
            return RegisterFactory(container, typeof(TFactory), name, selector, lifetime, injectionMembers);
        }
        
        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType)
        {
            return RegisterFactory(container, factoryType, new DefaultTypedFactoryComponentSelector());
        }

        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType, string name)
        {
            return RegisterFactory(container, factoryType, name, new DefaultTypedFactoryComponentSelector());
        }

        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType, ITypedFactoryComponentSelector selector)
        {
            return RegisterFactory(container, factoryType, null, selector);
        }

        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType, string name, ITypedFactoryComponentSelector selector)
        {
            return RegisterFactory(container, factoryType, name, selector, null);
        }

        public static IUnityContainer RegisterFactory(this IUnityContainer container, Type factoryType, string name, ITypedFactoryComponentSelector selector, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(factoryType, "factoryType");
            Guard.AssertNotNull(selector, "selector");

            Guard.AssertCondition(
                factoryType.IsInterface,
                factoryType,
                "TFactory",
                "Cannot generate an implementation for a non-interface factory type.");

            lifetime = lifetime ?? new TransientLifetimeManager();

            ITypedFactoryConfiguration configuration = container.Configure<ITypedFactoryConfiguration>();

            if(configuration == null)
            {
                throw new InvalidOperationException("TypedFactoryExtension must be registered with the container.");
            }

            configuration.RegisterFactory(factoryType, name, selector, lifetime, injectionMembers);

            return container;
        }
    }
}
