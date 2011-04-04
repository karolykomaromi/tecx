using System;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Common.Time;

namespace TecX.Unity.Lifetime
{
    public class CacheLifetimeManager : LifetimeManager, IDisposable
    {
        private object _value;
        private readonly ILease _lease;

        public CacheLifetimeManager(ILease lease)
        {
            Guard.AssertNotNull(lease, "lease");

            _lease = lease;
        }

        public override object GetValue()
        {
            RemoveValue();

            return _value;
        }

        public override void RemoveValue()
        {
            if (_lease.IsExpired)
            {
                Dispose();
            }
        }

        public override void SetValue(object newValue)
        {
            _value = newValue;
            _lease.Renew();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var d = _value as IDisposable;

                if (d != null)
                {
                    d.Dispose();
                }

                _value = null;
            }
        }

        public CacheLifetimeManager Clone()
        {
            CacheLifetimeManager clone = new CacheLifetimeManager(_lease)
                {
                    _value = this._value
                };

            return clone;
        }
    }
}
