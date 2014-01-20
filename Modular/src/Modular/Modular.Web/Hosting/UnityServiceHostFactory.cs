namespace Modular.Web.Hosting
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Microsoft.Practices.Unity;
    using Search;

    public class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer container;

        public UnityServiceHostFactory()
        {
            this.container = new UnityContainer();

            this.ConfigureContainer();
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