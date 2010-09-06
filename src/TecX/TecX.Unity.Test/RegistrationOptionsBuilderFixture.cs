using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Test.TestObjects;

using Microsoft.Practices.Unity;

namespace TecX.Unity.Test
{
    [TestClass]
    public class RegistrationOptionsBuilderFixture
    {
        public RegistrationOptionsBuilderFixture() { }

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
        // Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen. 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CanCreateRegistrationOptionsBuilder()
        {
            RegistrationOptionsBuilder builder = Then.Register();

            Assert.IsNotNull(builder);
        }

        [TestMethod]
        public void WhenRegisteredWithDefaultOptions_TypeIsRegisteredWithAllItsInterfaces()
        {
            RegistrationOptionsBuilder builder = Then.Register()
                .MappingTo<TestLogger>();

            IEnumerable<RegistrationOptions> options = builder.Build()
                .OrderBy(o => o.From.Name);

            Assert.AreEqual(2, options.Count());

            Assert.AreEqual(typeof(IDisposable), options.ElementAt(0).From);
            Assert.AreEqual(typeof(ILogger), options.ElementAt(1).From);
        }

        [TestMethod]
        public void WhenRegisteredWithDefaultOptions_TypeIsRegisteredWithoutNameUsingPerCallMode()
        {
            RegistrationOptionsBuilder builder = Then.Register()
                .MappingTo<TraceLogger>();

            RegistrationOptions options = builder.Build()
                .Single();

            Assert.AreEqual(string.Empty, options.Name);
            Assert.AreEqual(typeof(TraceLogger), options.To);
            Assert.AreEqual(typeof(TransientLifetimeManager), options.LifetimeManager.GetType());
            Assert.IsNull(options.InjectionMembers);
        }

        [TestMethod]
        public void WhenRegisteredWithTypeNameOption_TypeIsRegisteredWithTypeName()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .WithTypeName();

            var option = builder.Build().Single();

            Assert.AreEqual("TraceLogger", option.Name);
        }

        [TestMethod]
        public void WhenRegisteredWithIContractNameOption_TypeIsRegisteredWithTypeSpecificPrefix()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .WithIContractName();

            var option = builder.Build().Single();

            Assert.AreEqual("Trace", option.Name);
        }

        [TestMethod]
        public void WhenRegisteredWithWithoutPartNameOption_PartNameIsExcludedFromRegistration()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .WithoutPartName("Logger");

            var option = builder.Build().Single();

            Assert.AreEqual("Trace", option.Name);
        }

        [TestMethod]
        public void WhenRegisteredAsSpecificInterface_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TestLogger>()
                .As<ILogger>();

            RegistrationOptions options = builder.Build()
                .Single();

            Assert.AreEqual(typeof(ILogger), options.From);
        }

        [TestMethod]
        public void WhenRegisteredAsSingleInterfaceOfType_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .AsSingleInterfaceOfType();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(ILogger), option.From);
        }

        [TestMethod]
        public void WhenRegisteredAsAllInterfacesOfType_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TestLogger>()
                .AsAllInterfacesOfType();

            var options = builder.Build()
                .OrderBy(o => o.From.Name);

            Assert.AreEqual(typeof(IDisposable), options.ElementAt(0).From);
            Assert.AreEqual(typeof(ILogger), options.ElementAt(1).From);
        }

        [TestMethod]
        public void WhenRegisteredAsFirstInterfaceOfType_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TestLogger>()
                .AsFirstInterfaceOfType();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(ILogger), option.From);
        }

        [TestMethod]
        public void WhenRegisteredUsingPerCallMode_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .UsingPerCallMode();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(TransientLifetimeManager), option.LifetimeManager.GetType());
        }

        [TestMethod]
        public void WhenRegisteredUsingSingletonMode_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .UsingSingletonMode();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(ContainerControlledLifetimeManager), option.LifetimeManager.GetType());
        }

        [TestMethod]
        public void WhenRegisteredUsingPerThreadMode_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .UsingPerThreadMode();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(PerThreadLifetimeManager), option.LifetimeManager.GetType());
        }

        [TestMethod]
        public void WhenRegisteredUsingCustomLifetimeManager_RegisteredAsExpected()
        {
            var builder = Then.Register()
                .MappingTo<TraceLogger>()
                .UsingLifetime<DummyLifetimeManager>();

            var option = builder.Build().Single();

            Assert.AreEqual(typeof(DummyLifetimeManager), option.LifetimeManager.GetType());
        }
    }
}
