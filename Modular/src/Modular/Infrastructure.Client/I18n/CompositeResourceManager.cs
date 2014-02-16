namespace Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class CompositeResourceManager : IResourceManager
    {
        private readonly List<IResourceManager> resourceManagers;

        public CompositeResourceManager()
            : this(new IResourceManager[0])
        {
        }

        public CompositeResourceManager(params IResourceManager[] resourceManagers)
        {
            this.resourceManagers = new List<IResourceManager>(resourceManagers ?? new IResourceManager[0]);
        }

        public string GetString(string name, CultureInfo culture)
        {
            foreach (IResourceManager resourceManager in this.resourceManagers)
            {
                string value = resourceManager.GetString(name, culture);

                if (!string.Equals(name, value, StringComparison.Ordinal))
                {
                    return value;
                }
            }

            return name;
        }

        public void Add(IResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManagers.Add(resourceManager);
        }
    }
}