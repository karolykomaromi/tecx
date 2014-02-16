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

        public string this[ResxKey key]
        {
            get
            {
                foreach (IResourceManager resourceManager in this.resourceManagers)
                {
                    string value = resourceManager[key];

                    if (!string.Equals(key.ToString(), value, StringComparison.Ordinal))
                    {
                        return value;
                    }
                }

                return key.ToString();
            }
        }

        public string GetString(string name, CultureInfo culture)
        {
            return name.ToUpperInvariant();
        }

        public void Add(IResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManagers.Add(resourceManager);
        }
    }
}