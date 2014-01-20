namespace Search
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure;
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
            container.RegisterType<ISearchService, SearchServiceClient>(new InjectionConstructor());
            container.RegisterType<ICommand, SearchCommand>("search");
            container.RegisterType<ICommand, SearchSuggestionCommand>("suggestions");
            container.RegisterType<SearchViewModel>(new InjectionConstructor(new ResolvedParameter<ICommand>("search"), new ResolvedParameter<ICommand>("suggestions")));
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            if (this.TryGetViewFor<SearchViewModel>(out view))
            {
                regionManager.AddToRegion(Regions.Shell.TopLeft, view);
            }
        }
    }
}
