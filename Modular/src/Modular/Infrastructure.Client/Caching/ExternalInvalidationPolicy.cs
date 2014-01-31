namespace Infrastructure.Caching
{
    using System.Diagnostics.Contracts;
    using Infrastructure.Events;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class ExternalInvalidationPolicy : CacheItemPolicy, ISubscribeTo<CacheInvalidated>
    {
        private readonly CacheRegionName cacheRegionName;

        public ExternalInvalidationPolicy(CacheRegionName cacheRegionName)
        {
            this.cacheRegionName = cacheRegionName;
        }

        void ISubscribeTo<CacheInvalidated>.Handle(CacheInvalidated message)
        {
            Contract.Requires(message != null);

            if (message.CacheRegion == this.cacheRegionName || message.CacheRegion == CacheRegions.All)
            {
                this.AbsoluteExpiration = TimeProvider.Now - 1.Days();
            }
        }
    }
}