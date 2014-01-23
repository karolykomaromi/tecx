namespace Search
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
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

        public Module(IEntryPointInfo entryPointInfo)
            : base(entryPointInfo)
        {
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<SearchResultsViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResolvedParameter<ICommand>(Regions.Shell.Content)));
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

            container.RegisterType<ISearchService, SearchServiceClient>(new InjectionConstructor());

            container.RegisterInstance<INavigateAsync>(Regions.Shell.Content, this.RegionManager.Regions[Regions.Shell.Content]);
            container.RegisterType<ICommand, NavigationCommand>(
                Regions.Shell.Content, 
                new InjectionConstructor(new ResolvedParameter<INavigateAsync>(Regions.Shell.Content)));
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

            regionManager.AddToRegion(
                Regions.Shell.Navigation,
                new NavigationViewModel(
                    new NavigationCommand(regionManager.Regions[Regions.Shell.Content]))
                    {
                        Destination = new Uri("SearchResultsView", UriKind.Relative),
                        Name = this.ResourceManager["Search.Label_NavigationMenuEntry"]
                    });
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
