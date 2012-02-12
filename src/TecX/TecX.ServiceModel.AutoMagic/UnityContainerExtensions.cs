namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterForAutoDiscovery(this IUnityContainer container, Type contract, string name, LifetimeManager lifetime, Uri[] scopes, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(contract, "contract");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForAutoDiscovery(contract, name, lifetime, scopes, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterForAutoDiscovery<TTypeToBuild>(this IUnityContainer container, string name, params Uri[] scopes)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForAutoDiscovery<TTypeToBuild>(name, scopes);

            return container;
        }

        public static IUnityContainer RegisterForAutoDiscovery<TTypeToBuild>(this IUnityContainer container, params Uri[] scopes)
        {
            Guard.AssertNotNull(container, "container");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForAutoDiscovery<TTypeToBuild>(scopes);

            return container;
        }

        public static IUnityContainer RegisterForUsingAppConfig<TTypeToBuild>(this IUnityContainer container, string name, string endpointConfigName)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForUsingAppConfig<TTypeToBuild>(name, endpointConfigName);

            return container;
        }

        public static IUnityContainer RegisterForUsingAppConfig<TTypeToBuild>(this IUnityContainer container, string endpointConfigName)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForUsingAppConfig<TTypeToBuild>(endpointConfigName);

            return container;
        }

        public static IUnityContainer RegisterForManualSetup<TTypeToBuild>(this IUnityContainer container, string name, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForManualSetup<TTypeToBuild>(name, address, binding);

            return container;
        }

        public static IUnityContainer RegisterForManualSetup<TTypeToBuild>(this IUnityContainer container, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForManualSetup<TTypeToBuild>(address, binding);

            return container;
        }
    }
}
