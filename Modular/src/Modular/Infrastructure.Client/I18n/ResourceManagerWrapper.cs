namespace Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Resources;

    public class ResourceManagerWrapper : IResourceManager
    {
        private readonly ResourceManager resourceManager;

        public ResourceManagerWrapper(ResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManager = resourceManager;
        }

        public string this[ResxKey key]
        {
            get
            {
                return key.ToString();
            }
        }

        public string GetString(string name, CultureInfo culture)
        {
            return this.resourceManager.GetString(name, culture);
        }
    }
}