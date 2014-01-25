namespace Infrastructure.Caching
{
    using System;

    public class CacheInvalidationEventArgs : EventArgs
    {
        private readonly CacheRegionName cacheRegion;

        public CacheInvalidationEventArgs(CacheRegionName cacheRegion)
        {
            this.cacheRegion = cacheRegion;
        }

        public CacheRegionName CacheRegion
        {
            get
            {
                return this.cacheRegion;
            }
        }
    }
}