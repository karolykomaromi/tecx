namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using System.Linq;
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

            if (moduleResources != null && (moduleResources.Count > 0 || moduleResources.MergedDictionaries.Any()))
            {
                this.applicationResources.MergedDictionaries.Add(moduleResources);
            }
        }
    }
}