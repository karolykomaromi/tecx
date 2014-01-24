namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using AutoMapper;

    public class AutoMapperInitializer : IModuleInitializer
    {
        private readonly IMappingEngine mappingEngine;

        public AutoMapperInitializer(IMappingEngine mappingEngine)
        {
            Contract.Requires(mappingEngine != null);

            this.mappingEngine = mappingEngine;
        }

        public void Initialize(UnityModule module)
        {
            module.ConfigureMapping(this.mappingEngine);
        }
    }
}
