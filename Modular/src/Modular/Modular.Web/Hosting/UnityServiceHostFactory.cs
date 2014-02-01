namespace Modular.Web.Hosting
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Microsoft.Practices.Unity;
    using Search;

    public class UnityServiceHostFactory : ServiceHostFactory, IDisposable
    {
        private readonly IUnityContainer container;

        public UnityServiceHostFactory()
        {
            this.container = new UnityContainer();

            this.ConfigureContainer();
        }

        public void Dispose()
        {
            this.container.Dispose();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            UnityServiceHost host = new UnityServiceHost(this.container, serviceType, baseAddresses);

            return host;
        }

        private void ConfigureContainer()
        {
            this.container.RegisterType<ISearchService, SearchService>();
        }
    }
}