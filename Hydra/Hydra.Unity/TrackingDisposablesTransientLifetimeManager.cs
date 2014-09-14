namespace Hydra.Unity
{
    using System;
    using Microsoft.Practices.Unity;

    public class TrackingDisposablesTransientLifetimeManager : LifetimeManager, IDisposable
    {
        private IDisposable disposable;

        public override object GetValue()
        {
            return null;
        }

        public override void SetValue(object newValue)
        {
            IDisposable d = null;
            if ((d = newValue as IDisposable) != null)
            {
                this.disposable = d;
            }
        }

        public override void RemoveValue()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposable != null)
            {
                this.disposable.Dispose();
            }

            this.disposable = null;
        }
    }
}