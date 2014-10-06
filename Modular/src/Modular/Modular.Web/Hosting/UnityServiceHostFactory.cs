namespace Modular.Web.Hosting
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.Unity;
    using Modular.Web.Configuration;

    public class UnityServiceHostFactory : ServiceHostFactory, IDisposable
    {
        private readonly IUnityContainer container;
        private readonly LogWriter logger;

        public UnityServiceHostFactory()
        {
            this.container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();

            this.logger = this.container.Resolve<LogWriter>();
        }

        public void Dispose()
        {
            this.container.Dispose();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            string msg = string.Format(CultureInfo.CurrentCulture, "Creating host. ServiceType=\"{0}\" Addresses=\"{1}\"", serviceType.FullName, string.Join(";", baseAddresses.Select(uri => uri.ToString())));
            this.logger.Write(msg, "Debug");

            UnityServiceHost host = new UnityServiceHost(this.container, serviceType, baseAddresses);

            return host;
        }
    }
}