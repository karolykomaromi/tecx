namespace TecX.Unity.Lifetime
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Common.Time;

    public class CacheLifetimeManager : LifetimeManager, IDisposable
    {
        private readonly ILease lease;
        private object value;

        public CacheLifetimeManager(ILease lease)
        {
            Guard.AssertNotNull(lease, "lease");

            this.lease = lease;
        }

        public override object GetValue()
        {
            this.RemoveValue();

            return this.value;
        }

        public override void RemoveValue()
        {
            if (this.lease.IsExpired)
            {
                this.Dispose();
            }
        }

        public override void SetValue(object newValue)
        {
            this.value = newValue;
            this.lease.Renew();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Dispose(true);
        }

        public CacheLifetimeManager Clone()
        {
            CacheLifetimeManager clone = new CacheLifetimeManager(this.lease)
            {
                value = this.value
            };

            return clone;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var d = this.value as IDisposable;

                if (d != null)
                {
                    d.Dispose();
                }

                this.value = null;
            }
        }
    }
}
