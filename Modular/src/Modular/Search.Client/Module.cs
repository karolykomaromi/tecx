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
    using Infrastructure.Options;
    using Infrastructure.Views;
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

            container.RegisterType<ISearchService, SearchServiceClient>("client", new InjectionConstructor());
            container.RegisterType<ISearchService, DispatchingSearchServiceClient>(
                new InjectionConstructor(new ResolvedParameter<ISearchService>("client"), typeof(Dispatcher)));

            container.RegisterType<ICommand, NavigationCommand>(
                RegionNames.Shell.Content, 
                new InjectionConstructor(new ResolvedParameter<INavigateAsync>(RegionNames.Shell.Content)));

            container.RegisterType<SearchOptionsViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResxKey("SEARCH.LABEL_SEARCHOPTIONS")));
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement search;
            if (this.TryGetViewFor<SearchViewModel>(out search))
            {
                regionManager.AddToRegion(RegionNames.Shell.TopLeft, search);
            }

            FrameworkElement searchResult;
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResult, new Param("titleKey", new ResxKey("SEARCH.LABEL_SEARCHRESULTS"))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, searchResult);
            }

            FrameworkElement searchOptions;
            if (this.TryGetViewFor<SearchOptionsViewModel>(out searchOptions))
            {
                regionManager.AddToRegion(RegionNames.Main.Options, searchOptions);
            }

            NavigationView navigation = new NavigationBuilder()
                                                        .ToView(searchResult)
                                                        .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                        .WithLabel(new ResxKey("SEARCH.LABEL_SEARCHRESULTS"));

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
        }

        protected override IResourceManager CreateResourceManager()
        {
            IResourceManager rm = new ResxFilesResourceManager(typeof(Labels));

            return rm;
        }

        protected override IOptions CreateModuleOptions()
        {
            return this.Container.Resolve<SearchOptionsViewModel>();
        }

        protected override void ConfigureViewRules(IViewRuleEngine ruleEngine)
        {
            ruleEngine.Add(new DisableSearchRule());
        }
    }
}
