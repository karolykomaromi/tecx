namespace Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Commands;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Search.Service;

    public class Module : UnityModule
    {
        public Module(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
            Contract.Requires(container != null);
            Contract.Requires(regionManager != null);
            Contract.Requires(logger != null);
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<SearchResultsViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShowThings<IEnumerable<SearchResult>>, SearchResultsViewModel>();
            container.RegisterType<IShowThings<IEnumerable<SearchResult>>, DispatchingShowThings<IEnumerable<SearchResult>>>("dispatch");

            container.RegisterType<SearchViewModel>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ICommand>("search"), new ResolvedParameter<ICommand>("suggestions")));
            container.RegisterType<IShowThings<IEnumerable<string>>, SearchViewModel>();
            container.RegisterType<IShowThings<IEnumerable<string>>, DispatchingShowThings<IEnumerable<string>>>("dispatch");

            container.RegisterType<ISearchService, SearchServiceClient>(new InjectionConstructor());
            container.RegisterType<ICommand, SearchCommand>("search", new InjectionConstructor(typeof(ISearchService), typeof(ICommandManager), new ResolvedParameter<IShowThings<IEnumerable<SearchResult>>>("dispatch")));
            container.RegisterType<ICommand, SearchSuggestionCommand>("suggestions", new InjectionConstructor(typeof(ISearchService), new ResolvedParameter<IShowThings<IEnumerable<string>>>("dispatch")));
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement searchView;
            if (this.TryGetViewFor<SearchViewModel>(out searchView))
            {
                regionManager.AddToRegion(Regions.Shell.TopLeft, searchView);
            }

            FrameworkElement searchResultView;
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResultView))
            {
                regionManager.AddToRegion(Regions.Shell.Content, searchResultView);
            }
        }

        protected override ResourceDictionary CreateModuleResources()
        {
            Uri source = new Uri("/Search.Client;component/Resources.xaml", UriKind.Relative);

            ResourceDictionary resources = new ResourceDictionary { Source = source };

            return resources;
        }
    }
}
