namespace TecX.ServiceModel.Unity
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    /// <summary>
    /// ServiceHostFactory that can be used by IIS to host a service configured with
    /// Unity
    /// </summary>
    public abstract class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer container;

        public UnityServiceHostFactory()
        {
            this.container = new UnityContainer();

            this.ConfigureContainer(this.container);
        }

        public IUnityContainer Container
        {
            get { return this.container; }
        }

        protected abstract void ConfigureContainer(IUnityContainer container);

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Guard.AssertNotNull(serviceType, "serviceType");

            UnityServiceHost host = new UnityServiceHost(this.Container, serviceType, baseAddresses);

            return host;
        }
    }
}