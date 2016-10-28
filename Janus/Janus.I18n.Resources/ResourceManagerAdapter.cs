namespace Janus.I18n.Resources
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Resources;

    public class ResourceManagerAdapter : IResourceManager
    {
        private readonly ResourceManager resourceManager;

        public ResourceManagerAdapter(ResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManager = resourceManager;
        }

        public string GetString(string name, CultureInfo culture)
        {
            return this.resourceManager.GetString(name, culture);
        }

        public object GetObject(string name, CultureInfo culture)
        {
            return this.resourceManager.GetObject(name, culture);
        }
    }
}
