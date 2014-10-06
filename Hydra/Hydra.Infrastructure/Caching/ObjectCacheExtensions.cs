namespace Hydra.Infrastructure.Caching
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Caching;

    public static class ObjectCacheExtensions
    {
        public static IDisposable Add(this ObjectCache cache, string key, object value)
        {
            Contract.Requires(cache != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(key));
            Contract.Requires(value != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            ExternallyControlledChangeMonitor changeMonitor = new ExternallyControlledChangeMonitor();

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(changeMonitor);

            if (cache.Add(key, value, policy))
            {
                return new CacheItemInvalidationToken(changeMonitor);
            }

            return CacheItemInvalidationToken.Null;
        }
    }
}