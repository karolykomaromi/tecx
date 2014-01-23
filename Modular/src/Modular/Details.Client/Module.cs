namespace Details
{
    using System;
    using System.Windows;
    using Details.Assets.Resources;
    using Details.ViewModels;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Regions;

    public class Module : UnityModule
    {
        /// <summary>
        /// Details
        /// </summary>
        public static readonly string Name = "Details";

        public Module(IEntryPointInfo entryPointInfo)
            : base(entryPointInfo)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement detailsView;
            if (this.TryGetViewFor<ProductDetailsViewModel>(out detailsView))
            {
                this.RegionManager.AddToRegion(Regions.Shell.Content, detailsView);
            }

            regionManager.AddToRegion(
                Regions.Shell.Navigation,
                new NavigationViewModel(
                    new NavigationCommand(regionManager.Regions[Regions.Shell.Content]))
                        {
                            Destination = new Uri("ProductDetailsView", UriKind.Relative),
                            Name = this.ResourceManager["Details.Label_NavigationMenuEntry"]
                        });
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
