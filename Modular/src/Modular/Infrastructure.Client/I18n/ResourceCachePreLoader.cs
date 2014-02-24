namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Infrastructure.Caching;
    using Infrastructure.Entities;
    using Infrastructure.Events;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class ResourceCachePreLoader
    {
        private readonly IResourceService resourceService;
        private readonly IDisplayManager displayManager;
        private readonly IEventAggregator eventAggregator;

        public ResourceCachePreLoader(IResourceService resourceService, IDisplayManager displayManager, IEventAggregator eventAggregator)
        {
            Contract.Requires(resourceService != null);
            Contract.Requires(displayManager != null);
            Contract.Requires(eventAggregator != null);

            this.resourceService = resourceService;
            this.displayManager = displayManager;
            this.eventAggregator = eventAggregator;
        }

        public void WarmUp(ObjectCache cache, string moduleName)
        {
            Contract.Requires(cache != null);
            Contract.Requires(!string.IsNullOrEmpty(moduleName));

            this.displayManager.ShowBusy();
            this.resourceService.BeginGetStrings(moduleName, CultureInfo.CurrentCulture.TwoLetterISOLanguageName, 0, int.MaxValue, this.OnGetStringsCompleted, cache);
        }

        private void OnGetStringsCompleted(IAsyncResult ar)
        {
            try
            {
                ObjectCache cache = ar.AsyncState as ObjectCache;

                if (cache != null)
                {
                    ResourceString[] resources = this.resourceService.EndGetStrings(ar);

                    foreach (ResourceString s in resources)
                    {
                        ExternalInvalidationPolicy policy = new ExternalInvalidationPolicy(CacheRegions.Resources);

                        this.eventAggregator.Subscribe(policy);

                        cache.Add(s.Name, s.Value, policy);
                    }
                }
            }
            finally
            {
                this.displayManager.HideBusy();
            }
        }
    }
}
