namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    using Infrastructure.I18n;

    public class AppResourceAppender : IAppResourceAppender
    {
        private readonly ResourceDictionary appResourceDictionary;

        private readonly CompositeResourceManager appResourceManager;

        public AppResourceAppender(ResourceDictionary appResourceDictionary, CompositeResourceManager appResourceManager)
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