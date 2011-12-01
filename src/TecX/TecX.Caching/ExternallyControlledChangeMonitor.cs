namespace TecX.Caching
{
    using System;
    using System.Runtime.Caching;

    public class ExternallyControlledChangeMonitor : ChangeMonitor
    {
        private readonly string uniqueId;

        public ExternallyControlledChangeMonitor()
        {
            this.uniqueId = Guid.NewGuid().ToString();

            this.InitializationComplete();
        }

        public override string UniqueId
        {
            get
            {
                return this.uniqueId;
            }
        }

        public void Release()
        {
            this.OnChanged(null);
        }

        protected override void Dispose(bool disposing)
        {
            // TODO weberse 2011-12-01 unhook from external controller like event aggregator
        }
    }
}
