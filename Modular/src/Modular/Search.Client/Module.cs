namespace Search
{
    using System.Windows.Controls;
    using Infrastructure;
    using Infrastructure.Modularity;
    using Infrastructure.Options;
    using Infrastructure.Views;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Search.ViewModels;

    public class Module : UnityModule
    {
        /// <summary>
        /// Search
        /// </summary>
        public static readonly string Name = "Search";

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IRegionManager regionManager)
            : base(container, logger, moduleTracker, regionManager)
        {
        }

        public override string ModuleName
        {
            get { return Name; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control search;
            if (this.TryGetViewFor<SearchViewModel>(out search))
            {
                regionManager.AddToRegion(RegionNames.Shell.TopLeft, search);
            }

            Control searchResult;
            if (this.TryGetViewFor<SearchResultsViewModel>(out searchResult))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, searchResult);

                NavigationView navigation = new NavigationBuilder(this.RegionManager)
                    .ToView(searchResult)
                    .HideOn(Option.Create((SearchOptionsViewModel vm) => vm.IsSearchEnabled));

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            Control searchOptions;
            if (this.TryGetViewFor<SearchOptionsViewModel>(out searchOptions))
            {
                regionManager.AddToRegion(RegionNames.Main.Options, searchOptions);
            }
        }
    }
}
