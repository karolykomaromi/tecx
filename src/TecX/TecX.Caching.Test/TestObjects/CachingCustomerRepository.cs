namespace TecX.Caching.Test.TestObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;

    using TecX.Caching.KeyGeneration;
    using TecX.Caching.QueryInterception;
    using TecX.Common;
    using TecX.Common.Extensions.Primitives;

    public class CachingCustomerRepository : ICustomerRepository
    {
        private readonly ICustomerRepository inner;

        private readonly ObjectCache cache;

        private readonly QueryInterceptor<Customer> customers;

        // TODO weberse 2011-12-01 better use WeakReference here
        private readonly List<ExternallyControlledChangeMonitor> monitors;

        public CachingCustomerRepository(ICustomerRepository inner)
        {
            Guard.AssertNotNull(inner, "inner");

            this.inner = inner;
            this.cache = new MemoryCache(typeof(CachingCustomerRepository).Name);

            this.customers = new QueryInterceptor<Customer>(this.inner.Customers, new QueryInterceptorProvider(this.inner.Customers.Provider));

            this.customers.QueryProvider.Executing += this.OnQueryExecuting;
            this.customers.QueryProvider.Executed += this.OnQueryExecuted;

            this.monitors = new List<ExternallyControlledChangeMonitor>();
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

            this.monitors.ForEach(m => m.Release());
            this.monitors.Clear();
        }

        private void OnQueryExecuted(object sender, ExpressionExecuteEventArgs e)
        {
            // if we dont have the value in the cache add it
            string cacheKey = e.Expression.GetCacheKey();

            IQueryable<Customer> cachedResult = this.cache[cacheKey] as IQueryable<Customer>;

            if (cachedResult == null)
            {
                var evaluatedQueryable = ((IEnumerable<Customer>)e.Result).ToList().AsQueryable();

                // add value with expiration of 10 minutes and the ability to control when the item
                // is invalidated in case a write/update occurs
                CacheItem cacheItem = new CacheItem(cacheKey, evaluatedQueryable);

                CacheItemPolicy policy = new CacheItemPolicy { SlidingExpiration = 10.Minutes() };

                ExternallyControlledChangeMonitor monitor = new ExternallyControlledChangeMonitor();

                policy.ChangeMonitors.Add(monitor);

                this.monitors.Add(monitor);

                this.cache.Add(cacheItem, policy);
            }
        }

        private void OnQueryExecuting(object sender, ExpressionExecuteEventArgs e)
        {
            // check wether we have that value in the cache and return cached data instead
            string cacheKey = e.Expression.GetCacheKey();

            IQueryable<Customer> cachedResult = this.cache[cacheKey] as IQueryable<Customer>;

            if (cachedResult != null)
            {
                e.Handled = true;
                e.Result = cachedResult;
            }
        }
    }
}