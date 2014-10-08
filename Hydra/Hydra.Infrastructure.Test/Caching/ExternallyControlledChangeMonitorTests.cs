namespace Hydra.Infrastructure.Test.Caching
{
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;
    using Xunit;

    public class ExternallyControlledChangeMonitorTests
    {
        [Fact]
        public void Should_Invalidate_CacheItem()
        {
            ExternallyControlledChangeMonitor changeMonitor = new ExternallyControlledChangeMonitor();

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(changeMonitor);

            ObjectCache cache = new MemoryCache("TESTS");

            string key = "1";
            object expected = new object();

            Assert.True(cache.Add(key, expected, policy));

            Assert.Same(expected, cache[key]);

            changeMonitor.Invalidate();

            Assert.Null(cache[key]);
        }
    }
}
