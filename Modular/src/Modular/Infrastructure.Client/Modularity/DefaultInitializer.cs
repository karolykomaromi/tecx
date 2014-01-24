namespace Infrastructure.Modularity
{
    public class DefaultInitializer : CompositeInitializer
    {
        public DefaultInitializer(
            ContainerInitializer containerInitializer, 
            ResourceManagerInitializer resourceManagerInitializer, 
            ModuleResourcesInitializer moduleResourcesInitializer, 
            AutoMapperInitializer autoMapperInitializer,
            RegionInitializer regionInitializer)
            : base(containerInitializer, resourceManagerInitializer, moduleResourcesInitializer, autoMapperInitializer, regionInitializer)
        {
            ////this.ConfigureContainer(this.container);
            ////IResourceManager resourceManager = this.CreateResourceManager();
            ////this.ResourceAppender.Add(resourceManager);
            ////ResourceDictionary moduleResources = this.CreateModuleResources();
            ////this.ResourceAppender.Add(moduleResources);
            ////this.ConfigureRegions(this.RegionManager);
        }
    }
}