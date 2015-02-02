namespace Infrastructure.Caching
{
    public static class CacheRegions
    {
        /// <summary>
        /// RESOURCES
        /// </summary>
        public static readonly CacheRegionName Resources = new CacheRegionName("RESOURCES");

        /// <summary>
        /// ALL_REGIONS
        /// </summary>
        public static readonly CacheRegionName All = new CacheRegionName("ALL_REGIONS");
    }
}
