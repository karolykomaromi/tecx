﻿namespace Modular.Web.Configuration
{
    using Infrastructure.ListViews;
    using Infrastructure.ListViews.Filter;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.Unity;
    using Modular.Web.Hubs;
    using Search;

    public class UnityContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var configSource = new FileConfigurationSource("web.config", true);

            var configurator = new UnityContainerConfigurator(this.Container);

            EnterpriseLibraryContainer.ConfigureContainer(configurator, configSource);

            this.Container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ISearchService, SearchService>();
            this.Container.RegisterType<IResourceKeyProvider, ResourceKeyProvider>();
            this.Container.RegisterInstance<IPropertyFilter>(new PropertyFilter("Id"));
        }
    }
}