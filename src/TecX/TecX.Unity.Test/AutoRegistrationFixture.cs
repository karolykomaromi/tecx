using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Unity.AutoRegistration;
using TecX.Unity.Test.Contract;
using TecX.Unity.Test.TestObjects;
using TecX.TestTools.Extensions;

namespace TecX.Unity.Test
{

    [TestClass]
    public class AutoRegistrationFixture
    {
        private Mock<IUnityContainer> _containerMock;
        private List<RegisterEvent> _registered;
        private IUnityContainer _container;
        private delegate void RegistrationCallback(Type from, Type to, string name, LifetimeManager lifetime, InjectionMember[] ims);
        private IUnityContainer _realContainer;

        private static string _knownExternalAssembly = "Microsoft.Practices.Unity.Interception";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //TODO weberse get the current path, climb up until you reach src, get up one more
            //level then descend to lib\Unity2 and then load the PIAB assembly

            string[] directories = context.DeploymentDirectory.Split(new[] { @"\" },
                StringSplitOptions.RemoveEmptyEntries);

            string unityLibFolder = string.Empty;

            int index = directories.IndexOf("trunk");

            //access to modified closure is alright here
            LiteralExtensions.Times((directories.Length - index - 1), () => unityLibFolder += @"..\");

            unityLibFolder += @"lib\Unity2\";

            _knownExternalAssembly = unityLibFolder + _knownExternalAssembly;
        }

        [TestInitialize]
        public void SetUp()
        {
            _realContainer = new UnityContainer();

            _containerMock = new Mock<IUnityContainer>();
            _registered = new List<RegisterEvent>();
            var setup = _containerMock
                .Setup(c => c.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<LifetimeManager>()));
            var callback = new RegistrationCallback((from, to, name, lifetime, ips) =>
            {
                _registered.Add(new RegisterEvent(from, to, name, lifetime));
                _realContainer.RegisterType(from, to, name, lifetime);
            });

            // Using reflection, because current version of Moq doesn't support callbacks with more than 4 arguments
            setup
                .GetType()
                .GetMethod("SetCallbackWithArguments", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(setup, new object[] { callback });

            _container = _containerMock.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenContainerIsNullThrowsException()
        {
            _container = null;
            _container
                .ConfigureAutoRegistration();
        }

        [TestMethod]
        public void WhenApplingAutoRegistrationWithoutAnyRulesNothingIsRegistered()
        {
            _container
                .ConfigureAutoRegistration()
                .ApplyAutoRegistration();
            Assert.IsFalse(_registered.Any());
        }

        [TestMethod]
        public void WhenApplingAutoRegistrationWithOnlyAssemblyRulesNothingIsRegistered()
        {
            _container
                .ConfigureAutoRegistration()
                .ApplyAutoRegistration();
            Assert.IsFalse(_registered.Any());
        }

        [TestMethod]
        public void WhenApplyMethodIsNotCalledAutoRegistrationDoesNotHappen()
        {
            _container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register());

            Assert.IsFalse(_registered.Any());
        }

        [TestMethod]
        public void WhenAssemblyIsExcludedAutoRegistrationDoesNotHappenForItsTypes()
        {
            _container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register())
                .ExcludeAssemblies(If.ContainsType<TestCache>)
                .ApplyAutoRegistration();

            Assert.IsFalse(_registered.Any());
        }

        [TestMethod]
        public void WhenExternalAssemblyIsLoadedAutoRegistrationHappensForItsTypes()
        {
            _container
                .ConfigureAutoRegistration()
                .LoadAssemblyFrom(String.Format("{0}.dll", _knownExternalAssembly))
                .ExcludeSystemAssemblies()
                .Include(If.Any, Then.Register())
                .ApplyAutoRegistration();

            Assert.IsTrue(_registered.Any());
        }

        [TestMethod]
        public void WhenTypeIsExcludedAutoRegistrationDoesNotHappenForIt()
        {
            _container
                .ConfigureAutoRegistration()
                .Exclude(If.Is<TestCache>)
                .Include(If.Is<TestCache>, Then.Register())
                .ApplyAutoRegistration();

            Assert.IsFalse(_registered.Any());
        }

        [TestMethod]
        public void WhenRegisterWithDefaultOptionsTypeMustBeRegisteredAsAllInterfacesItImplementsUsingPerCallLifetimeWithEmptyName()
        {
            _container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register())
                .ApplyAutoRegistration();

            Assert.IsTrue(_registered.Count == 2);

            var iCacheRegisterEvent = _registered.SingleOrDefault(r => r.From == typeof(ICache));
            var iDisposableRegisterEvent = _registered.SingleOrDefault(r => r.From == typeof(IDisposable));

            Assert.IsNotNull(iCacheRegisterEvent);
            Assert.IsNotNull(iDisposableRegisterEvent);
            Assert.AreEqual(typeof(TestCache), iCacheRegisterEvent.To);
            Assert.AreEqual(typeof(TransientLifetimeManager), iCacheRegisterEvent.Lifetime.GetType());
            Assert.AreEqual(String.Empty, iCacheRegisterEvent.Name);
            Assert.AreEqual(typeof(TestCache), iDisposableRegisterEvent.To);
            Assert.AreEqual(typeof(TransientLifetimeManager), iDisposableRegisterEvent.Lifetime.GetType());
            Assert.AreEqual(String.Empty, iDisposableRegisterEvent.Name);
        }

        [TestMethod]
        public void WhenRegistrationObjectIsPassedRequestedTypeRegisteredAsExpected()
        {
            const string registrationName = "TestName";

            var registration = Then.Register();
            registration.Interfaces = new[] { typeof(ICache) };
            registration.LifetimeManager = new ContainerControlledLifetimeManager();
            registration.Name = registrationName;

            _container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, registration)
                .ApplyAutoRegistration();

            Assert.AreEqual(1, _registered.Count);
            var registerEvent = _registered.Single();
            Assert.AreEqual(typeof(TestCache), registerEvent.To);
            Assert.AreEqual(typeof(ContainerControlledLifetimeManager), registerEvent.Lifetime.GetType());
            Assert.AreEqual(registrationName, registerEvent.Name);
        }

        [TestMethod]
        public void WhenHaveMoreThanOneRegistrationRulesTypesRegisteredAsExpected()
        {
            _container
                .ConfigureAutoRegistration()
                .Include(If.Implements<ICustomerRepository>,
                         Then.Register()
                             .AsSingleInterfaceOfType()
                             .WithTypeName()
                             .UsingPerThreadMode())
                .Include(If.DecoratedWith<LoggerAttribute>, Then.Register().AsAllInterfacesOfType())
                .ApplyAutoRegistration();

            // 2 types implement ICustomerRepository, LoggerAttribute decorated type implement 2 interfaces
            Assert.AreEqual(4, _registered.Count);
        }

        [TestMethod]
        public void WhenImplementsITypeNameMehtodCalledItWorksAsExpected()
        {
            Assert.IsTrue(typeof(CustomerRepository).ImplementsITypeName());
            Assert.IsTrue(typeof(Introduction).ImplementsITypeName());
        }

        [TestMethod]
        public void WhenImplementsOpenGenericTypesRegisteredAsExpected()
        {
            _container
                .ConfigureAutoRegistration()
                .Include(type => type.ImplementsOpenGeneric(typeof(IHandlerFor<>)),
                    Then.Register().AsFirstInterfaceOfType().WithTypeName())
                .ApplyAutoRegistration();

            Assert.AreEqual(2, _registered.Count);
            Assert.IsTrue(_registered
                .Select(r => r.To)
                .SequenceEqual(new[] { typeof(DomainEventHandlerOne), typeof(DomainEventHandlerTwo) }));
            Assert.IsTrue(_registered
                .Select(r => r.From)
                .All(t => t == typeof(IHandlerFor<DomainEvent>)));

            Assert.AreEqual(2, _realContainer.ResolveAll(typeof(IHandlerFor<DomainEvent>)).Count());
        }

        [TestMethod]
        public void WhenWithPartNameMehtodCalledItWorksAsExpected()
        {
            Assert.AreEqual(
                "Customer",
                new RegistrationOptions { Type = typeof(CustomerRepository) }
                    .WithPartName(WellKnownAppParts.DesignPatterns.Repository)
                    .Name);

            Assert.AreEqual(
                "Test",
                new RegistrationOptions { Type = typeof(TestCache) }
                    .WithPartName("Cache")
                    .Name);
        }

        private class RegisterEvent
        {
            public Type From { get; private set; }
            public Type To { get; private set; }
            public string Name { get; private set; }
            public LifetimeManager Lifetime { get; private set; }

            public RegisterEvent(Type from, Type to, string name, LifetimeManager lifetime)
            {
                From = from;
                To = to;
                Name = name;
                Lifetime = lifetime;
            }
        }

        public class Introduction : IIntroduction
        {

        }

        public interface IIntroduction
        {
        }

        private void Example()
        {
            var container = new UnityContainer();

            container
                .ConfigureAutoRegistration()
                .LoadAssemblyFrom("MyFancyPlugin.dll")
                .ExcludeSystemAssemblies()
                .ExcludeAssemblies(a => a.GetName().FullName.Contains("Test"))
                .Include(If.ImplementsSingleInterface, Then.Register().AsSingleInterfaceOfType().UsingSingletonMode())
                .Include(If.Implements<ILogger>, Then.Register().UsingPerCallMode())
                .Include(If.ImplementsITypeName, Then.Register().WithTypeName())
                .Include(If.Implements<ICustomerRepository>, Then.Register().WithName("Sample"))
                .Include(If.Implements<IOrderRepository>,
                         Then.Register().AsSingleInterfaceOfType().UsingPerCallMode())
                .Include(If.DecoratedWith<LoggerAttribute>,
                         Then.Register()
                             .As<IDisposable>()
                             .WithPartName(WellKnownAppParts.General.Logger)
                             .UsingLifetime<MyLifetimeManager>())
                .Exclude(t => t.Name.Contains("Trace"))
                .ApplyAutoRegistration();
        }
    }
}