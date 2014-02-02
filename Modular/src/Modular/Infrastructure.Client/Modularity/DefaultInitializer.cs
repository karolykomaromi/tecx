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
            RegionInitializer regionInitializer)
            : base(containerInitializer, resourceManagerInitializer, moduleResourcesInitializer, autoMapperInitializer, viewRuleInitializer, regionInitializer)
        {
        }
    }
}