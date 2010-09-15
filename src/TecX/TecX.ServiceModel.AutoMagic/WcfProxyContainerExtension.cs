using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.ServiceModel.AutoMagic
{
    public class WcfProxyContainerExtension : UnityContainerExtension, IWcfProxyConfiguration
    {
        #region Overrides of UnityContainerExtension

        protected override void Initialize()
        {
            /* intentionally left blank */
        }

        #endregion Overrides of UnityContainerExtension

        #region IWcfProxyConfiguration Members

        /// <summary>
        /// Registers the type with the <see cref="WcfProxyContainerExtension"/>
        /// </summary>
        /// <typeparam name="TTypeToBuild">The type of the type to build.</typeparam>
        /// <param name="name">Registers a named mapping that can be resolved via
        ///   <code>container.Resolve{TTypeToBuild}("name")</code></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public IUnityContainerExtensionConfigurator RegisterType<TTypeToBuild>(string name, params Uri[] scopes)
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
        public IUnityContainerExtensionConfigurator RegisterType<TTypeToBuild>(params Uri[] scopes)
        {
            return RegisterType<TTypeToBuild>(null, scopes);
        }

        #endregion
    }
}