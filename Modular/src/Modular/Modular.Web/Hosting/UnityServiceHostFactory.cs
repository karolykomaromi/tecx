using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using Search;

namespace Modular.Web.Hosting
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer container;

        public UnityServiceHostFactory()
        {
            this.container = new UnityContainer();

            this.ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            this.container.RegisterType<ISearchService, SearchService>();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            UnityServiceHost host = new UnityServiceHost(this.container, serviceType, baseAddresses);

            return host;
        }
    }
}