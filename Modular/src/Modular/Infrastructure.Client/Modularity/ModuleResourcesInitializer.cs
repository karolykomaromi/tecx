namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class ModuleResourcesInitializer : IModuleInitializer
    {
        private readonly ResourceDictionary applicationResources;

        public ModuleResourcesInitializer(ResourceDictionary applicationResources)
        {
            Contract.Requires(applicationResources != null);

            this.applicationResources = applicationResources;
        }

        public void Initialize(UnityModule module)
        {
            ResourceDictionary moduleResources = module.CreateModuleResources();

            if (moduleResources != null && 
                moduleResources.Count > 0)
            {
                this.applicationResources.MergedDictionaries.Add(moduleResources);
            }
        }
    }
}