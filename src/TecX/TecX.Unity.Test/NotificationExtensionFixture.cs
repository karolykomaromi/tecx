namespace TecX.Unity.Test
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Notification;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class NotificationExtensionFixture
    {
        [TestMethod]
        public void GetNotificationOnCtorException()
        {
            var container = new UnityContainer();

            bool notified = false;

            container.AddExtension(new NotificationExtension((ex, ctx, policy) => notified = true));

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

            container.AddExtension(new NotificationExtension((ex, ctx, policy) => notified = true));
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

            container.AddExtension(new NotificationExtension((ex, ctx, policy) => notified = true));

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

            container.AddExtension(new NotificationExtension((ex, ctx, policy) => notified = true));

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

        [TestMethod]
        public void GetNotificationOnObjectCreating()
        {
            var container = new UnityContainer();

            var extension = new NotificationExtension();

            bool creating = false;

            extension.Creating += (s, e) => creating = true;

            container.AddExtension(extension);

            container.Resolve<Foo>();

            Assert.IsTrue(creating);
        }

        [TestMethod]
        public void GetNotificationOnObjectCreated()
        {
            var container = new UnityContainer();

            var extension = new NotificationExtension();

            bool created = false;

            extension.Created += (s, e) => created = true;

            container.AddExtension(extension);

            container.Resolve<Foo>();

            Assert.IsTrue(created);
        }
    }
}
