using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    /// <summary>
    /// ServiceHostFactory that can be used by IIS to host a service configured with
    /// Unity
    /// </summary>
    public abstract class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceHostFactory()
        {
            _container = new UnityContainer();

            ConfigureContainer(_container);
        }

        public IUnityContainer Container
        {
            get { return _container; }
        }

        protected abstract void ConfigureContainer(IUnityContainer container);

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Guard.AssertNotNull(serviceType, "serviceType");

            UnityServiceHost host = new UnityServiceHost(Container, serviceType, baseAddresses);

            return host;
        }
    }
}