namespace TecX.Caching
{
    using System;
    using System.Runtime.Caching;

    public class ExternallyControlledChangeMonitor : ChangeMonitor
    {
        private readonly string uniqueId;

        private ExpirationToken expirationToken;

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

        public ExpirationToken ExpirationToken
        {
            get
            {
                return this.expirationToken;
            }

            set
            {
                if (this.expirationToken != null)
                {
                    this.expirationToken.Expired -= this.OnExpired;
                }

                this.expirationToken = value;

                if (this.expirationToken != null)
                {
                    this.expirationToken.Expired += this.OnExpired;
                }
            }
        }

        public void Release()
        {
            this.OnChanged(null);
        }

        protected override void Dispose(bool disposing)
        {
            this.ExpirationToken = null;
            this.Release();
        }

        private void OnExpired(object sender, EventArgs e)
        {
            this.Release();
        }
    }
}
