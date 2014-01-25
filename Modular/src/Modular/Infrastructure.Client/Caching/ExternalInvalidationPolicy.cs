namespace Infrastructure.Caching
{
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class ExternalInvalidationPolicy : CacheItemPolicy
    {
        private readonly CacheRegionName cacheRegionName;

        public ExternalInvalidationPolicy(CacheRegionName cacheRegionName)
        {
            this.cacheRegionName = cacheRegionName;
        }

        public void OnCacheInvalidated(object sender, CacheInvalidationEventArgs args)
        {
            if (args.CacheRegion == this.cacheRegionName || args.CacheRegion == CacheRegions.All)
            {
                this.AbsoluteExpiration = TimeProvider.Now - 1.Days();
            }
        }
    }
}