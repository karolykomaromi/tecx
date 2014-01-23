namespace Infrastructure.Caching
{
    using System;
    using System.Windows.Threading;

    public class CacheInvalidationManager : Alerting, ICacheInvalidationManager
    {
        public CacheInvalidationManager(Dispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public event EventHandler Invalidated
        {
            add { this.AddWeakReferenceHandler(value); }

            remove { this.RemoveWeakReferenceHandler(value); }
        }

        public void RequestInvalidate()
        {
            this.RaiseEvent();
        }
    }
}