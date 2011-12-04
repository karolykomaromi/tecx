namespace TecX.Caching
{
    using System;

    public class ExpirationToken
    {
        public event EventHandler Expired = delegate { };

        public void Expire()
        {
            this.Expired(this, EventArgs.Empty);
        }
    }
}
