namespace TecX.Web.Unity.Test.TestObjects
{
    using System;
    using System.Reflection;
    using System.Web;

    class MockHttpApplication : HttpApplication
    {
        public void RaiseEndRequest()
        {
            FieldInfo field = typeof(HttpApplication).GetField("EventEndRequest", BindingFlags.NonPublic | BindingFlags.Static);

            object key = field.GetValue(this);

            Delegate @delegate = this.Events[key];

            @delegate.DynamicInvoke(this, EventArgs.Empty);
        }
    }
}