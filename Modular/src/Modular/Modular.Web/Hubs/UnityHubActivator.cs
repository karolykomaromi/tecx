namespace Modular.Web.Hubs
{
    using System.Diagnostics.Contracts;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Practices.Unity;
    using Modular.Web.Configuration;

    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer container;

        public UnityHubActivator()
        {
            this.container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();
        }

        public IHub Create(HubDescriptor descriptor)
        {
            Contract.Requires(descriptor != null);
            Contract.Requires(descriptor.HubType != null);

            object hub = this.container.Resolve(descriptor.HubType);

            return (IHub)hub;
        }
    }
}