﻿using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

using Moq;

using TecX.Unity.Registration;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class AutoRegistrationBuilderFixture
    {
        private class RegistrationEvent
        {
            public Type From { get; set; }
            public Type To { get; set; }
            public string Name { get; set; }
            public LifetimeManager LifetimeManager { get; set; }
            public InjectionMember[] InjectionMembers { get; set; }
        }

        private readonly Mock<IUnityContainer> _mockContainer;
        private readonly IUnityContainer _container;

        private readonly List<RegistrationEvent> _registrationEvents;
        private readonly List<UnityContainerExtension> _containerExtensions;

        public AutoRegistrationBuilderFixture() 
        {
            _registrationEvents = new List<RegistrationEvent>();
            _containerExtensions = new List<UnityContainerExtension>();

            _mockContainer = new Mock<IUnityContainer>();

            _mockContainer
                .Setup(c => c.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<string>(),
                    It.IsAny<LifetimeManager>(), It.Is<InjectionMember[]>(im => true)))
                .Callback<Type, Type, string, LifetimeManager, InjectionMember[]>(
                    (from, to, name, lifetimeManager, injectionMembers) =>
                    {
                        RegistrationEvent registrationEvent = new RegistrationEvent 
                        { 
                            From = from,
                            To = to,
                            Name = name,
                            LifetimeManager = lifetimeManager,
                            InjectionMembers = injectionMembers
                        };

                        _registrationEvents.Add(registrationEvent);
                    });

            _mockContainer
                .Setup(c => c.AddExtension(It.IsAny<UnityContainerExtension>()))
                .Callback<UnityContainerExtension>(ext => _containerExtensions.Add(ext));

            _container = _mockContainer.Object;
        }

        public TestContext TestContext { get; set; }

        #region Zusätzliche Testattribute
        //
        // Sie können beim Schreiben der Tests folgende zusätzliche Attribute verwenden:
        //
        // Verwenden Sie ClassInitialize, um vor Ausführung des ersten Tests in der Klasse Code auszuführen.
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Verwenden Sie ClassCleanup, um nach Ausführung aller Tests in einer Klasse Code auszuführen.
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
         //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen. 
         [TestInitialize]
         public void TestInitialize() 
         {
             _registrationEvents.Clear();
             _containerExtensions.Clear();
         }

        //
        // Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CanCreateRegistrationBuilder()
        {
            IRegistry registry = _container.ConfigureRegistrations();

            Assert.IsNotNull(registry);
        }

        [TestMethod]
        public void WhenRegistering_MappingIsRegisteredWithContainer()
        {
            var builder = _container.ConfigureRegistrations();

            builder
                .ExcludeUnitTestAssemblies()
                .Include(If.Is<TraceLogger>(), Then.Register())
                .ApplyRegistrations();

            Assert.AreEqual(1, _registrationEvents.Count);

            RegistrationEvent registration = _registrationEvents[0];

            Assert.AreEqual(string.Empty, registration.Name);
            Assert.AreEqual(typeof(TraceLogger), registration.To);
            Assert.AreEqual(typeof(ILogger), registration.From);
            Assert.AreEqual(typeof(TransientLifetimeManager), registration.LifetimeManager.GetType());
            Assert.AreEqual(0, registration.InjectionMembers.Length);
        }

        [TestMethod]
        public void WhenInterceptionIsConfigured_AddedAsExpected()
        {
            IRegistry builder = _container
                .ConfigureRegistrations()
                .ExcludeUnitTestAssemblies()
                .EnableInterception()
                .ApplyRegistrations();

            Assert.AreEqual(1, _containerExtensions.Count);
            Assert.AreEqual(typeof(Interception), _containerExtensions[0].GetType());
        }

        [TestMethod]
        public void WhenAddingExtension_CanConfigureItAsExpected()
        {
            IUnityContainer container = new UnityContainer();

            IRegistry registration = container
                .ConfigureRegistrations()
                .ExcludeUnitTestAssemblies()
                .Include(The.Extension<TestExtension>()
                             .WithConfiguration<ITestExtensionConfig>(c => c.Prop1 = true))
                .ApplyRegistrations();

            TestExtension extension = container.Resolve<TestExtension>();

            Assert.IsTrue(extension.Prop1);
        }
    }
}
