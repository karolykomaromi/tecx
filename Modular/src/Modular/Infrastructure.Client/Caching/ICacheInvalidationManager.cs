namespace Infrastructure.Caching
{
    using System;

    public interface ICacheInvalidationManager
    {
        event EventHandler<CacheInvalidationEventArgs> CacheInvalidated;

        void RequestInvalidate(CacheRegionName cacheRegion);
    }
}