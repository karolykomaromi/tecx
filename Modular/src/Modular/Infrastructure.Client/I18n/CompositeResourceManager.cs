namespace Infrastructure.I18n
{
    using System;

    public class CompositeResourceManager : IResourceManager
    {
        private readonly IResourceManager[] resourceManagers;

        public CompositeResourceManager(params IResourceManager[] resourceManagers)
        {
            this.resourceManagers = resourceManagers ?? new IResourceManager[0];
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
    }
}