namespace TecX.Caching.Test
{
    using System.Linq;
    using System.Runtime.Caching;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TecX.Caching.Test.TestObjects;

    [TestClass]
    public class CachingFixture
    {
        [TestMethod]
        public void CallingCachedQueryDoesNotCauseAdditionalLoadFromSource()
        {
            var mock = new Mock<ICustomerRepository>();

            mock.SetupGet(r => r.Customers).Returns(
                new[]
                    {
                        new Customer { Id = 1, Name = "1" }, 
                        new Customer { Id = 2, Name = "2" },
                        new Customer { Id = 3, Name = "3" }
                    }.AsQueryable());

            var cache = new CachingCustomerRepository(mock.Object);

            var r1 = cache.Customers.Where(c => c.Id < 3).ToList();
            var r2 = cache.Customers.Where(c => c.Id < 3).ToList();
            var r3 = cache.Customers.Where(c => c.Id < 3).ToList();
            var r4 = cache.Customers.Where(c => c.Id < 3).ToList();

            CollectionAssert.AreEqual(r1, r2);
            CollectionAssert.AreEqual(r2, r3);
            CollectionAssert.AreEqual(r3, r4);

            mock.VerifyGet(r => r.Customers, Times.Once());
        }

        [TestMethod]
        public void CanUseExternallyControlledChangeMonitor()
        {
            ObjectCache cache = new MemoryCache("test");

            object value = new object();

            CacheItem item = new CacheItem("1", value);

            CacheItemPolicy policy = new CacheItemPolicy();

            var monitor = new ExternallyControlledChangeMonitor();

            policy.ChangeMonitors.Add(monitor);

            cache.Add(item, policy);

            Assert.AreEqual(value, cache["1"]);

            monitor.Release();

            Assert.IsNull(cache["1"]);
        }
    }
}
