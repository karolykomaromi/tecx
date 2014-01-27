namespace Recipe
{
    using System.Windows;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class Module : UnityModule
    {
        /// <summary>
        /// Recipe
        /// </summary>
        public static readonly string Name = "Recipe";

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;

            ResxKey titleKey = new ResxKey("DUMMY");
            ListViewName listViewName = new ListViewName("DUMMY");

            if (this.TryGetViewFor<DynamicListViewModel>(out view, new Parameter("listViewName", listViewName), new Parameter("listViewTitleKey", titleKey)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, view);

                NavigationViewModel navigation = new NavigationBuilder()
                                                    .ToView(view)
                                                    .WithParameter("name", listViewName.ToString())
                                                    .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                    .WithLabel(titleKey);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }
        }
    }
}
