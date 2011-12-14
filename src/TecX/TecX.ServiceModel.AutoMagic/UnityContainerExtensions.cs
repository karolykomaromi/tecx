namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(this IUnityContainer container, string name, params Uri[] scopes)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForAutoDiscovery<TTypeToBuild>(name, scopes);

            return config;
        }

        public static IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(this IUnityContainer container, params Uri[] scopes)
        {
            Guard.AssertNotNull(container, "container");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForAutoDiscovery<TTypeToBuild>(scopes);

            return config;
        }

        public static IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(this IUnityContainer container, string name, string endpointConfigName)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForUsingAppConfig<TTypeToBuild>(name, endpointConfigName);

            return config;
        }

        public static IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(this IUnityContainer container, string endpointConfigName)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForUsingAppConfig<TTypeToBuild>(endpointConfigName);

            return config;
        }

        public static IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(this IUnityContainer container, string name, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForManualSetup<TTypeToBuild>(name, address, binding);

            return config;
        }

        public static IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(this IUnityContainer container, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            var config = container.Configure<IWcfProxyConfiguration>();

            config.RegisterForManualSetup<TTypeToBuild>(address, binding);

            return config;
        }
    }
}
