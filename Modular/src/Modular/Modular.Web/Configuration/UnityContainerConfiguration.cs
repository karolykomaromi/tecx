namespace Modular.Web.Configuration
{
    using Infrastructure;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Practices.Unity;
    using Modular.Web.Hubs;
    using Search;

    public class UnityContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ISearchService, SearchService>();
            this.Container.RegisterType<IResourceKeyProvider, ResourceKeyProvider>();
        }
    }
}