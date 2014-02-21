namespace Search
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.UnityExtensions.Injection;
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

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IModuleInitializer initializer)
            : base(container, logger, moduleTracker, initializer)
        {
        }

        public override string ModuleName
        {
            get { return Name; }
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<SearchResultsViewModel>(
                new ContainerControlledLifetimeManager(),
                new SmartConstructor(new ResolvedParameter<ICommand>(RegionNames.Shell.Content)));

            container.RegisterType<SearchViewModel>(new ContainerControlledLifetimeManager(), new SmartConstructor());

            container.RegisterType<ICommand, SearchCommand>("searchCommand");
            container.RegisterType<ICommand, SuggestionsCommand>("suggestionsCommand");

            container.RegisterType<ISearchService, SearchServiceClient>(new InjectionConstructor(typeof(Dispatcher)));

            container.RegisterType<ICommand, NavigationCommand>(
                RegionNames.Shell.Content,
                new InjectionConstructor(new ResolvedParameter<INavigateAsync>(RegionNames.Shell.Content)));

            container.RegisterType<SearchOptionsViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResourceAccessor(() => Labels.SearchOptions)));
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control search;
            if (this.TryGetViewFor<SearchViewModel>(out search))
            {
                regionManager.AddToRegion(RegionNames.Shell.TopLeft, search);
            }

            Control searchResult;
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResult, new Parameter("title", new ResourceAccessor(() => Labels.SearchResults))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, searchResult);
            }

            Control searchOptions;
            if (this.TryGetViewFor<SearchOptionsViewModel>(out searchOptions))
            {
                regionManager.AddToRegion(RegionNames.Main.Options, searchOptions);
            }

            NavigationView navigation = new NavigationBuilder()
                                                        .ToView(searchResult)
                                                        .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                        .WithTitle(() => Labels.SearchResults)
                                                        .OnOptionsChanged((msg, vm) =>
                                                            {
                                                                var options = msg.Options as SearchOptionsViewModel;

                                                                if (options != null)
                                                                {
                                                                    if (options.IsSearchEnabled)
                                                                    {
                                                                        vm.Show();
                                                                    }
                                                                    else
                                                                    {
                                                                        vm.Hide();
                                                                    }
                                                                }
                                                            });

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
        }
    }
}
