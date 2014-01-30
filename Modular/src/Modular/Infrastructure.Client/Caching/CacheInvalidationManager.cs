namespace Infrastructure.Caching
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Events;
    using Microsoft.Practices.Prism.Logging;

    public class CacheInvalidationManager : Alerting<CacheInvalidationEventArgs>, ICacheInvalidationManager
    {
        public CacheInvalidationManager(Dispatcher dispatcher, ILoggerFacade logger)
            : base(dispatcher, logger)
        {
        }

        public event EventHandler<CacheInvalidationEventArgs> CacheInvalidated
        {
            add { this.AddHandler(value); }

            remove { this.RemoveHandler(value); }
        }

        public void RequestInvalidate(CacheRegionName cacheRegion)
        {
            this.RaiseEvent(new CacheInvalidationEventArgs(cacheRegion));
        }
    }
}