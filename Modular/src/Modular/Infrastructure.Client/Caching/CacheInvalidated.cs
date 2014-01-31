namespace Infrastructure.Caching
{
    public class CacheInvalidated
    {
        private readonly CacheRegionName cacheRegion;

        public CacheInvalidated(CacheRegionName cacheRegion)
        {
            this.cacheRegion = cacheRegion;
        }

        public CacheRegionName CacheRegion
        {
            get { return cacheRegion; }
        }
    }
}