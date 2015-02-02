namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [DebuggerDisplay("BaseName={resourceManager.BaseName}")]
    public class ResourceManagerWrapper : IResourceManager
    {
        private readonly System.Resources.ResourceManager resourceManager;

        public ResourceManagerWrapper(System.Resources.ResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManager = resourceManager;
        }

        public string BaseName
        {
            get { return this.resourceManager.BaseName; }
        }

        public string GetString(string name, CultureInfo culture)
        {
            return this.resourceManager.GetString(name, culture);
        }
    }
}