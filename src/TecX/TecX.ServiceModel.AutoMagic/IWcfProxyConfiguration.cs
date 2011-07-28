using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Microsoft.Practices.Unity;

namespace TecX.ServiceModel.AutoMagic
{
    /// <summary>
    /// configuration interface exposed by the Wcf Proxy extension, it will be used to register new types in the container
    /// </summary>
    public interface IWcfProxyConfiguration : IUnityContainerExtensionConfigurator
    {
        /// <summary>
        /// Registers the type with the <see cref="WcfProxyContainerExtension"/>
        /// </summary>
        /// <typeparam name="TTypeToBuild">The type of the type to build.</typeparam>
        /// <param name="name">Registers a named mapping that can be resolved via
        ///   <code>container.Resolve{TTypeToBuild}("name")</code></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(string name, params Uri[] scopes);

        /// <summary>
        /// Registers the type with the <see cref="WcfProxyContainerExtension"/>
        /// </summary>
        /// <param name="scopes"></param>
        /// <typeparam name="TTypeToBuild">The type of the type to build.</typeparam>
        /// <returns>A wcf client that uses auto-discovery to find the service</returns>
        IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(params Uri[] scopes);

        IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string name, string endpointConfigName);

        IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string endpointConfigName);

        IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(string name, EndpointAddress address, Binding binding);

        IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(EndpointAddress address, Binding binding);
    }
}