namespace Infrastructure.Modularity
{
    using Infrastructure.I18n;

    public class ResourceManagerInitializer : IModuleInitializer
    {
        private readonly CompositeResourceManager applicationResourceManager;

        public ResourceManagerInitializer(CompositeResourceManager applicationResourceManager)
        {
            this.applicationResourceManager = applicationResourceManager;
        }

        public void Initialize(UnityModule module)
        {
            IResourceManager resourceManager = module.CreateResourceManager();

            if (resourceManager != null && 
                resourceManager as EchoResourceManager == null)
            {
                this.applicationResourceManager.Add(resourceManager);
            }
        }
    }
}