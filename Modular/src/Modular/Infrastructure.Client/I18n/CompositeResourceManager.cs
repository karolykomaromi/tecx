namespace Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class CompositeResourceManager : IResourceManager
    {
        private readonly List<IResourceManager> resourceManagers;

        public CompositeResourceManager(params IResourceManager[] resourceManagers)
        {
            this.resourceManagers = new List<IResourceManager>(resourceManagers ?? new IResourceManager[0]);
        }

        public string this[string key]
        {
            get
            {
                foreach (IResourceManager resourceManager in this.resourceManagers)
                {
                    string value = resourceManager[key];

                    if (!string.Equals(key, value, StringComparison.Ordinal))
                    {
                        return value;
                    }
                }

                return key;
            }
        }

        public void Add(IResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);

            this.resourceManagers.Add(resourceManager);
        }
    }
}