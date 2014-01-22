using Details.ViewModels;
using Infrastructure.Commands;
using Infrastructure.ViewModels;

namespace Details
{
    using System;
    using System.Windows;
    using Infrastructure;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class Module : UnityModule
    {
        public Module(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement detailsView;
            if (this.TryGetViewFor<ProductDetailsViewModel>(out detailsView))
            {
                this.RegionManager.AddToRegion(Regions.Shell.Content, detailsView);
            }

            regionManager.AddToRegion(Regions.Shell.Navigation,
                                      new NavigationViewModel(
                                          new NavigationCommand(regionManager.Regions[Regions.Shell.Content]))
                                      {
                                          Destination = new Uri("ProductDetailsView", UriKind.Relative),
                                          Name = "Product Details"
                                      });
        }

        protected override ResourceDictionary CreateModuleResources()
        {
            Uri source = new Uri("/Details.Client;component/Resources.xaml", UriKind.Relative);

            ResourceDictionary resources = new ResourceDictionary { Source = source };

            return resources;
        }
    }
}
