namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Prism.Regions;

    public class RegionInitializer : IModuleInitializer
    {
        private readonly IRegionManager regionManager;

        public RegionInitializer(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);

            this.regionManager = regionManager;
        }

        public void Initialize(UnityModule module)
        {
            module.ConfigureRegions(this.regionManager);
        }
    }
}