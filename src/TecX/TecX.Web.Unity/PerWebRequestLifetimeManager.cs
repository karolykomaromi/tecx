namespace TecX.Web.Unity
{
    using System;
    using System.Web;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public sealed class PerWebRequestLifetimeManager : LifetimeManager, IDisposable
    {
        private readonly string key;

        public PerWebRequestLifetimeManager()
        {
            this.key = "PerWebRequestLifetimeManager_" + Guid.NewGuid();

            HttpApplication application;
            if (HttpContext.Current != null &&
                (application = HttpContext.Current.ApplicationInstance) != null)
            {
                application.EndRequest += this.OnEndRequest;
            }
        }

        public override object GetValue()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                return context.Items[this.key];
            }

            return null;
        }

        public override void SetValue(object newValue)
        {
            Guard.AssertNotNull(newValue, "newValue");

            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                context.Items[this.key] = newValue;
            }
        }

        public override void RemoveValue()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                context.Items.Remove(this.key);
            }
        }

        public void Dispose()
        {
            this.RemoveValue();
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            this.RemoveValue();
        }
    }
}