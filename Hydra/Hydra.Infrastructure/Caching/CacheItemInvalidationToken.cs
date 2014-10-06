namespace Hydra.Infrastructure.Caching
{
    using System;
    using System.Diagnostics.Contracts;

    public class CacheItemInvalidationToken : IDisposable
    {
        public static readonly IDisposable Null = new NullCacheItemInvalidationToken();

        private readonly ExternallyControlledChangeMonitor changeMonitor;

        public CacheItemInvalidationToken(ExternallyControlledChangeMonitor changeMonitor)
        {
            Contract.Requires(changeMonitor != null);
            this.changeMonitor = changeMonitor;
        }

        public void Dispose()
        {
            this.changeMonitor.Invalidate();
        }

        private class NullCacheItemInvalidationToken : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}