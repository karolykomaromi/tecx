namespace TecX.Unity.Test
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Exception;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class ReportCreationExceptionExtensionFixture
    {
        [TestMethod]
        public void GetNotificationOnCtorException()
        {
            var container = new UnityContainer();

            bool notified = false;

            container.AddExtension(new ReportCreationExceptionExtension(ex => notified = true));

            try
            {
                container.Resolve<AlwaysThrows>();
            }
            catch (ResolutionFailedException)
            {
                // not interested in Unity's resolution failed exception
            }

            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void GetNotificationOnCtorExceptionWhenMappingTakesPlace()
        {
            var container = new UnityContainer();

            bool notified = false;

            container.AddExtension(new ReportCreationExceptionExtension(ex => notified = true));
            container.RegisterType<IAlwaysThrows, AlwaysThrows>();

            try
            {
                container.Resolve<IAlwaysThrows>();
            }
            catch (ResolutionFailedException)
            {
                // not interested in Unity's resolution failed exception
            }

            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void GetNotificationOnInjectionFactoryException()
        {
            var container = new UnityContainer();

            bool notified = false;

            container.AddExtension(new ReportCreationExceptionExtension(ex => notified = true));

            Func<IUnityContainer, object> factory = c => { throw new Exception("Bang!"); };
            container.RegisterType<AlwaysThrows>(new InjectionFactory(factory));
            try
            {
                container.Resolve<AlwaysThrows>();
            }
            catch(ResolutionFailedException)
            {
                // not interested in Unity's resolution failed exception
            }

            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void GetNotificationOnInjectionConstructorException()
        {
            var container = new UnityContainer();

            bool notified = false;

            container.AddExtension(new ReportCreationExceptionExtension(ex => notified = true));

            container.RegisterType<AlwaysThrows>(new InjectionConstructor());
            try
            {
                container.Resolve<AlwaysThrows>();
            }
            catch (ResolutionFailedException)
            {
                // not interested in Unity's resolution failed exception
            }

            Assert.IsTrue(notified);
        }
    }
}
