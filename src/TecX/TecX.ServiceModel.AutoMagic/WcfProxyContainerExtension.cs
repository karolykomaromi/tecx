namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class WcfProxyContainerExtension : UnityContainerExtension, IWcfProxyConfiguration
    {
        /// <summary>
        /// Registers the type with the <see cref="WcfProxyContainerExtension"/>
        /// </summary>
        /// <typeparam name="TTypeToBuild">The type of the type to build.</typeparam>
        /// <param name="name">Registers a named mapping that can be resolved via
        ///   <code>container.Resolve{TTypeToBuild}("name")</code></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(string name, params Uri[] scopes)
        {
            AutoDiscoveryBuildPlanPolicy policy = new AutoDiscoveryBuildPlanPolicy(scopes);

            Context.Policies.Set<IBuildPlanPolicy>(policy, NamedTypeBuildKey.Make<TTypeToBuild>(name));

            return this;
        }

        /// <summary>
        /// Registers the type with the <see cref="WcfProxyContainerExtension"/>
        /// </summary>
        /// <param name="scopes"></param>
        /// <typeparam name="TTypeToBuild">The type of the type to build.</typeparam>
        /// <returns>A wcf client that uses auto-discovery to find the service</returns>
        public IWcfProxyConfiguration RegisterForAutoDiscovery<TTypeToBuild>(params Uri[] scopes)
        {
            return RegisterForAutoDiscovery<TTypeToBuild>(null, scopes);
        }

        public IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string name, string endpointConfigName)
        {
            Guard.AssertNotNull(endpointConfigName, "endpointConfigName");

            AppConfigBuildPlanPolicy policy = new AppConfigBuildPlanPolicy(endpointConfigName);

            Context.Policies.Set<IBuildPlanPolicy>(policy, NamedTypeBuildKey.Make<TTypeToBuild>(name));

            return this;
        }

        public IWcfProxyConfiguration RegisterForUsingAppConfig<TTypeToBuild>(string endpointConfigName)
        {
            return RegisterForUsingAppConfig<TTypeToBuild>(null, endpointConfigName);
        }

        public IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(string name, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            ManualSetupBuildPlanPolicy policy = new ManualSetupBuildPlanPolicy(address, binding);

            Context.Policies.Set<IBuildPlanPolicy>(policy, NamedTypeBuildKey.Make<TTypeToBuild>(name));

            return this;
        }

        public IWcfProxyConfiguration RegisterForManualSetup<TTypeToBuild>(EndpointAddress address, Binding binding)
        {
            return RegisterForManualSetup<TTypeToBuild>(null, address, binding);
        }

        protected override void Initialize()
        {
            /* intentionally left blank */
        }
    }
}