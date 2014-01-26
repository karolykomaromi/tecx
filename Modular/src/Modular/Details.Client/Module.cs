namespace Details
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Threading;
    using Details.Assets.Resources;
    using Details.ViewModels;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class Module : UnityModule
    {
        /// <summary>
        /// Details
        /// </summary>
        public static readonly string Name = "Details";

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement detailsView;
            if (this.TryGetViewFor<ProductDetailsViewModel>(out detailsView))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, detailsView);
            }

            NavigationViewModel navigationViewModel = new NavigationBuilder()
                                                        .ToView(detailsView)
                                                        .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                        .WithLabel(new ResxKey("DETAILS.LABEL_NAVIGATIONMENUENTRY"));

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigationViewModel);
        }

        protected override IResourceManager CreateResourceManager()
        {
            return new ResxFilesResourceManager(typeof(Labels));
        }

        protected override ResourceDictionary CreateModuleResources()
        {
            Uri source = new Uri("/Details.Client;component/Resources.xaml", UriKind.Relative);

            ResourceDictionary resources = new ResourceDictionary { Source = source };

            return resources;
        }
    }
}
