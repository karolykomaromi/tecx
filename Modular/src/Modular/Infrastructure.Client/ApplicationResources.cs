namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    using Infrastructure.I18n;

    public class ApplicationResources : IApplicationResources
    {
        private readonly ResourceDictionary appResourceDictionary;

        private readonly CompositeResourceManager appResourceManager;

        public ApplicationResources(ResourceDictionary appResourceDictionary, CompositeResourceManager appResourceManager)
        {
            Contract.Requires(appResourceDictionary != null);
            Contract.Requires(appResourceManager != null);

            this.appResourceDictionary = appResourceDictionary;
            this.appResourceManager = appResourceManager;
        }

        public void Add(ResourceDictionary resourceDictionary)
        {
            this.appResourceDictionary.MergedDictionaries.Add(resourceDictionary);
        }

        public void Add(IResourceManager resourceManager)
        {
            this.appResourceManager.Add(resourceManager);
        }
    }
}