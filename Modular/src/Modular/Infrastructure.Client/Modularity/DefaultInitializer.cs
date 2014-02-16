namespace Infrastructure.Modularity
{
    public class DefaultInitializer : CompositeInitializer
    {
        public DefaultInitializer(
            ContainerInitializer containerInitializer, 
            ModuleResourcesInitializer moduleResourcesInitializer, 
            AutoMapperInitializer autoMapperInitializer,
            OptionsInitializer optionsInitializer,
            RegionInitializer regionInitializer)
            : base(
                containerInitializer, 
                moduleResourcesInitializer, 
                autoMapperInitializer,
                optionsInitializer, 
                regionInitializer)
        {
        }
    }
}