namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using Infrastructure.Caching;
    using Infrastructure.Events;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class CachingResourceManager : IResourceManager
    {
        private readonly IResourceManager inner;
        private readonly IEventAggregator eventAggregator;
        private readonly ObjectCache cache;

        public CachingResourceManager(IResourceManager inner, IEventAggregator eventAggregator, ObjectCache cache)
        {
            Contract.Requires(inner != null);
            Contract.Requires(eventAggregator != null);
            Contract.Requires(cache != null);

            this.inner = inner;
            this.eventAggregator = eventAggregator;
            this.cache = cache;
        }

        public string this[ResxKey key]
        {
            get
            {
                string cached = this.cache[key.ToString()] as string;

                if (cached != null)
                {
                    return (string)cached;
                }

                string value = this.inner[key];

                if (!string.Equals(key.ToString(), value, StringComparison.Ordinal))
                {
                    ExternalInvalidationPolicy policy = new ExternalInvalidationPolicy(CacheRegions.Resources);

                    this.eventAggregator.Subscribe(policy);

                    this.cache.Add(key.ToString(), value, policy);

                    return value;
                }

                return key.ToString();
            }
        }
    }
}