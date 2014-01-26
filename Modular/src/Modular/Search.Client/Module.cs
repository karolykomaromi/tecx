namespace Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using AutoMapper;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Search.Assets.Resources;
    using Search.Commands;
    using Search.Entities;
    using Search.ViewModels;

    public class Module : UnityModule
    {
        /// <summary>
        /// Search
        /// </summary>
        public static readonly string Name = "Search";

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<SearchResultsViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResolvedParameter<ICommand>(RegionNames.Shell.Content), typeof(IMappingEngine)));
            container.RegisterType<IShowThings<IEnumerable<SearchResult>>, SearchResultsViewModel>();
            container.RegisterType<IShowThings<IEnumerable<SearchResult>>, DispatchingShowThings<IEnumerable<SearchResult>>>("dispatch");

            container.RegisterType<SearchViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResolvedParameter<ICommand>("search"), new ResolvedParameter<ICommand>("suggestions")));

            container.RegisterType<ICommand, SearchCommand>(
                "search", 
                new InjectionConstructor(typeof(ISearchService), typeof(ICommandManager), new ResolvedParameter<IShowThings<IEnumerable<SearchResult>>>("dispatch")));

            // would result in a cyclic dependency viewmodel needs command need viewmodel, thus we break the circle by introducing a lazy step in between
            container.RegisterType<ICommand, SuggestionsCommand>(
                "suggestions", 
                new InjectionConstructor(typeof(ISearchService), new ResolvedParameter<IShowThings<IEnumerable<string>>>("dispatch")));

            container.RegisterType<IShowThings<IEnumerable<string>>, SearchViewModel>();
            container.RegisterType<IShowThings<IEnumerable<string>>, LazyShowThings<IEnumerable<string>>>("lazy");
            container.RegisterType<IShowThings<IEnumerable<string>>, DispatchingShowThings<IEnumerable<string>>>(
                "dispatch", 
                new InjectionConstructor(new ResolvedParameter<IShowThings<IEnumerable<string>>>("lazy"), typeof(Dispatcher)));

            this.Container.RegisterType<ISearchService, SearchServiceClient>("client", new InjectionConstructor());
            this.Container.RegisterType<ISearchService, DispatchingSearchServiceClient>(
                new InjectionConstructor(new ResolvedParameter<ISearchService>("client"), typeof(Dispatcher)));

            container.RegisterType<ICommand, NavigationCommand>(
                RegionNames.Shell.Content, 
                new InjectionConstructor(new ResolvedParameter<INavigateAsync>(RegionNames.Shell.Content)));
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement searchView;
            if (this.TryGetViewFor<SearchViewModel>(out searchView))
            {
                regionManager.AddToRegion(RegionNames.Shell.TopLeft, searchView);
            }

            FrameworkElement searchResultView;
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResultView))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, searchResultView);
            }

            NavigationViewModel navigationViewModel = new NavigationBuilder()
                                                        .ToView(searchResultView)
                                                        .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                        .WithLabel(new ResxKey("SEARCH.LABEL_NAVIGATIONMENUENTRY"));

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigationViewModel);
        }

        protected override IResourceManager CreateResourceManager()
        {
            IResourceManager rm = new ResxFilesResourceManager(typeof(Labels));

            return rm;
        }

        protected override ResourceDictionary CreateModuleResources()
        {
            Uri source = new Uri("/Search.Client;component/Resources.xaml", UriKind.Relative);

            ResourceDictionary resources = new ResourceDictionary { Source = source };

            return resources;
        }
    }
}
