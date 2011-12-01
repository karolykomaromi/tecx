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

            Assert.IsTrue(r1.SequenceEqual(r2));
            Assert.IsTrue(r2.SequenceEqual(r3));
            Assert.IsTrue(r3.SequenceEqual(r4));

            mock.VerifyGet(r => r.Customers, Times.Exactly(2));
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
