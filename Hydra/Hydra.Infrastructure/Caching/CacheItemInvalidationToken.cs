namespace Hydra.Infrastructure.Caching
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Logging;

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
            try
            {
                this.changeMonitor.Invalidate();
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Warning(ex);
            }
        }

        private class NullCacheItemInvalidationToken : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}