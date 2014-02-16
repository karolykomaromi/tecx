namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Infrastructure.Caching;
    using Infrastructure.Events;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
    using Microsoft.Practices.Prism.Logging;

    public class CachingResourceManager : IResourceManager
    {
        private readonly IResourceManager inner;
        private readonly IEventAggregator eventAggregator;
        private readonly ObjectCache cache;
        private readonly ILoggerFacade logger;

        public CachingResourceManager(IResourceManager inner, IEventAggregator eventAggregator, ObjectCache cache, ILoggerFacade logger)
        {
            Contract.Requires(inner != null);
            Contract.Requires(eventAggregator != null);
            Contract.Requires(cache != null);
            Contract.Requires(logger != null);

            this.inner = inner;
            this.eventAggregator = eventAggregator;
            this.cache = cache;
            this.logger = logger;
        }

        public string this[ResxKey key]
        {
            get
            {
                string cached = this.cache[key.ToString()] as string;

                if (cached != null)
                {
                    string msg = string.Format("Cache hit. Key=\"{0}\" Value=\"{1}\"", key, cached);
                    this.logger.Log(msg, Category.Debug, Priority.Low);
                    return cached;
                }

                this.logger.Log(string.Format("Cache hit. Key=\"{0}\"", key), Category.Debug, Priority.Low);
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

        public string GetString(string name, CultureInfo culture)
        {
            return name.ToUpperInvariant();
        }
    }
}