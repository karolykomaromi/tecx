namespace TecX.Caching.Test.TestObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;

    using TecX.Caching.QueryInterception;
    using TecX.Common;
    using TecX.Common.Extensions.Primitives;

    public class CachingCustomerRepository : ICustomerRepository
    {
        private readonly ICustomerRepository inner;

        private readonly ObjectCache cache;

        private readonly QueryInterceptor<Customer> customers;

        private readonly ExpirationToken expirationToken;

        public CachingCustomerRepository(ICustomerRepository inner)
        {
            Guard.AssertNotNull(inner, "inner");

            this.inner = inner;
            this.cache = new MemoryCache(typeof(CachingCustomerRepository).Name);

            this.customers = new QueryInterceptor<Customer>(this.inner.Customers);

            this.customers.QueryProvider.Executing += this.OnQueryExecuting;
            this.customers.QueryProvider.Executed += this.OnQueryExecuted;
            this.expirationToken = new ExpirationToken();
        }

        public IQueryable<Customer> Customers
        {
            get
            {
                return this.customers;
            }
        }

        public void Add(Customer customer)
        {
            Guard.AssertNotNull(customer, "customer");

            this.inner.Add(customer);

            this.expirationToken.Expire();
        }

        private void OnQueryExecuted(object sender, ExpressionExecuteEventArgs e)
        {
            Guard.AssertNotEmpty(e.CacheKey, "e.CacheKey");

            // if we dont have the value in the cache add it
            IQueryable<Customer> cachedResult = this.cache[e.CacheKey] as IQueryable<Customer>;

            if (cachedResult == null)
            {
                var evaluatedQueryable = ((IEnumerable<Customer>)e.Result).ToList().AsQueryable();

                // add value with expiration of 10 minutes and the ability to control when the item
                // is invalidated in case a write/update occurs
                CacheItem cacheItem = new CacheItem(e.CacheKey, evaluatedQueryable);

                CacheItemPolicy policy = new CacheItemPolicy { SlidingExpiration = 1.Minutes() };

                ExternallyControlledChangeMonitor monitor = new ExternallyControlledChangeMonitor
                    {
                        ExpirationToken = this.expirationToken
                    };

                policy.ChangeMonitors.Add(monitor);
                
                this.cache.Add(cacheItem, policy);
            }
        }

        private void OnQueryExecuting(object sender, ExpressionExecuteEventArgs e)
        {
            Guard.AssertNotEmpty(e.CacheKey, "e.CacheKey");

            // check wether we have that value in the cache and return cached data instead
            IQueryable<Customer> cachedResult = this.cache[e.CacheKey] as IQueryable<Customer>;

            if (cachedResult != null)
            {
                e.Handled = true;
                e.Result = cachedResult;
            }
        }
    }
}