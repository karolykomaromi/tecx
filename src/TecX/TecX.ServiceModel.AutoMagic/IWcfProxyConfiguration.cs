namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// configuration interface exposed by the Wcf Proxy extension, it will be used to register new types in the container
    /// </summary>
    public interface IWcfProxyConfiguration : IUnityContainerExtensionConfigurator
    {
        IWcfProxyConfiguration RegisterForAutoDiscovery(
            Type contract,
            string name,
            LifetimeManager lifetime,
            Uri[] scopes,
            params InjectionMember[] injectionMembers);

        IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(string name, params Uri[] scopes);

        IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(params Uri[] scopes);

        IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string name, string endpointConfigName);

        IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string endpointConfigName);

        IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(string name, EndpointAddress address, Binding binding);

        IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(EndpointAddress address, Binding binding);
    }
}