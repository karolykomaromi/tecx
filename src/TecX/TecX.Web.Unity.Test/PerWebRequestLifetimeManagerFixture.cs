namespace TecX.Web.Unity.Test
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PerWebRequestLifetimeManagerFixture
    {
        [TestMethod]
        public void RemoveOnEndRequest()
        {
            HttpContext context = new HttpContext(
                new HttpRequest(string.Empty, "http://tempuri.org", string.Empty),
                new HttpResponse(new StringWriter()));

            ////// User is logged in
            ////HttpContext.Current.User = new GenericPrincipal(
            ////    new GenericIdentity("username"),
            ////    new string[0]
            ////    );

            ////// User is logged out
            ////HttpContext.Current.User = new GenericPrincipal(
            ////    new GenericIdentity(String.Empty),
            ////    new string[0]
            ////    );

            HttpContext.Current = context;

            MockHttpApplication application = new MockHttpApplication();

            application.Init();

            context.ApplicationInstance = application;

            var container = new UnityContainer();

            container.RegisterType<IFoo, Foo>(new PerWebRequestLifetimeManager());

            IFoo foo = container.Resolve<IFoo>();

            IFoo foo2 = container.Resolve<IFoo>();

            Assert.AreSame(foo, foo2);

            application.RaiseEndRequest();

            IFoo foo3 = container.Resolve<IFoo>();

            Assert.AreNotSame(foo, foo3);
        }
    }

    public interface IFoo { }

    public class Foo : IFoo { }

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
