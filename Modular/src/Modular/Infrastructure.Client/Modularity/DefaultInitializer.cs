namespace Infrastructure.Modularity
{
    public class DefaultInitializer : CompositeInitializer
    {
        public DefaultInitializer(
            ContainerInitializer containerInitializer, 
            ResourceManagerInitializer resourceManagerInitializer, 
            ModuleResourcesInitializer moduleResourcesInitializer, 
            AutoMapperInitializer autoMapperInitializer,
            OptionsInitializer optionsInitializer,
            RegionInitializer regionInitializer)
            : base(
                containerInitializer, 
                resourceManagerInitializer, 
                moduleResourcesInitializer, 
                autoMapperInitializer,
                optionsInitializer, 
                regionInitializer)
        {
        }
    }
}