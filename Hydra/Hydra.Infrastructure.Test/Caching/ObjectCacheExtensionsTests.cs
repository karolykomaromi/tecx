namespace Hydra.Infrastructure.Test.Caching
{
    using System;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;
    using Xunit;

    public class ObjectCacheExtensionsTests
    {
        [Fact]
        public void Should_Return_Token_That_Invalidates_CacheItem_On_Dispose()
        {
            ObjectCache cache = new MemoryCache("TESTS");

            string key = "1";
            object expected = new object();

            using (IDisposable token = cache.Add(key, expected))
            {
                Assert.Same(expected, cache[key]);
            }

            Assert.Null(cache[key]);
        }
    }
}