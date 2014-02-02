namespace Search
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using AutoMapper;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.Dynamic;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Search.Assets.Resources;
    using Search.Commands;
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
                new InjectionConstructor(new ResolvedParameter<ICommand>(RegionNames.Shell.Content), typeof(IMappingEngine), typeof(ResxKey)));

            container.RegisterType<SearchViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("search"), 
                    new ResolvedParameter<ICommand>("suggestions"), 
                    typeof(ISearchService), 
                    typeof(IEventAggregator)));

            container.RegisterType<ICommand, SearchCommand>("search");
            container.RegisterType<ICommand, SuggestionsCommand>("suggestions");

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
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResultView, new Param("titleKey", new ResxKey("Search.Label_SearchResults"))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, searchResultView);
            }

            FrameworkElement searchOptionsView;
            if (this.TryGetViewFor<SearchOptionsViewModel>(out searchOptionsView))
            {
                regionManager.AddToRegion(RegionNames.Main.Options, searchOptionsView);
            }

            NavigationViewModel navigationViewModel = new NavigationBuilder()
                                                        .ToView(searchResultView)
                                                        .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                        .WithLabel(new ResxKey("SEARCH.LABEL_SEARCHRESULTS"));

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

        protected override void ConfigureViewRules(IViewRuleEngine ruleEngine)
        {
            ruleEngine.Add(new DisableSearchRule());
        }
    }
}
