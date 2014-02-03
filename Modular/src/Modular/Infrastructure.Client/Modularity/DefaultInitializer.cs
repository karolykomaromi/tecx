namespace Infrastructure.Modularity
{
    public class DefaultInitializer : CompositeInitializer
    {
        public DefaultInitializer(
            ContainerInitializer containerInitializer, 
            ResourceManagerInitializer resourceManagerInitializer, 
            ModuleResourcesInitializer moduleResourcesInitializer, 
            AutoMapperInitializer autoMapperInitializer,
            ViewRuleInitializer viewRuleInitializer,
            OptionsInitializer optionsInitializer,
            RegionInitializer regionInitializer)
            : base(
                containerInitializer, 
                resourceManagerInitializer, 
                moduleResourcesInitializer, 
                autoMapperInitializer, 
                viewRuleInitializer, 
                optionsInitializer, 
                regionInitializer)
        {
        }
    }
}