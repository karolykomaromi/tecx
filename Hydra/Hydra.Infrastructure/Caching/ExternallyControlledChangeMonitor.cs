namespace Hydra.Infrastructure.Caching
{
    using System;
    using System.Runtime.Caching;

    public class ExternallyControlledChangeMonitor : ChangeMonitor
    {
        private readonly string uniqueId;

        public ExternallyControlledChangeMonitor()
        {
            this.uniqueId = Guid.NewGuid().ToString(FormatStrings.Guid.Digits);

            this.InitializationComplete();
        }

        public override string UniqueId
        {
            get { return this.uniqueId; }
        }

        public void Invalidate()
        {
            if (!this.IsDisposed)
            {
                this.OnChanged(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
