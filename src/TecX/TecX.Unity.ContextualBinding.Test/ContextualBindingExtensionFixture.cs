using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.ContextualBinding;
using TecX.Unity.ContextualBinding.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class ContextualBindingExtensionFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void WhenTypeIsRegisteredWithBindingConstraint_OnlyResolvesOnMatch()
        {
            UnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TestLogger>(
                request => typeof(SomeService) == request.TypeToBuild);

            var svc = container.Resolve<SomeService>();

            Assert.IsNotNull(svc);
            Assert.IsNotNull(svc.Logger);
            Assert.IsInstanceOfType(svc.Logger, typeof(TestLogger));

            var svc2 = container.Resolve<AnotherService>();
        }

        [TestMethod]
        public void WhenTypeIsRegisteredWithBindingContraint_ResolvesProperly()
        {
            UnityContainer container = new UnityContainer();
            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TestLogger>(
                request => typeof(SomeService) == request.TypeToBuild);

            container.RegisterType<ILogger, TraceLogger>(
                request => typeof(AnotherService) == request.TypeToBuild);

            var svc = container.Resolve<SomeService>();
            Assert.IsNotNull(svc.Logger);
            Assert.IsInstanceOfType(svc.Logger, typeof(TestLogger));

            var svc2 = container.Resolve<AnotherService>();
            Assert.IsNotNull(svc2.Logger);
            Assert.IsInstanceOfType(svc2.Logger, typeof(TraceLogger));
        }

        [TestMethod]
        public void WhenDefaultMappingIsRegistered_DoesNotOverwriteExistingContextualBindingMapping()
        {
            UnityContainer container = new UnityContainer();
            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TestLogger>(
                request => typeof(SomeService) == request.TypeToBuild);

            container.RegisterType<ILogger, TraceLogger>();

            var svc = container.Resolve<SomeService>();

            Assert.IsNotNull(svc.Logger);
            Assert.IsInstanceOfType(svc.Logger, typeof(TestLogger));
        }

        [TestMethod]
        public void WhenContextualBindingMappingIsRegistered_DoesNotOverwriteExistingDefaultBinding()
        {
            UnityContainer container = new UnityContainer();
            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TraceLogger>();

            container.RegisterType<ILogger, TestLogger>(
                request => typeof(SomeService) == request.TypeToBuild);

            var svc = container.Resolve<AnotherService>();

            Assert.IsNotNull(svc.Logger);
            Assert.IsInstanceOfType(svc.Logger, typeof(TraceLogger));
        }

        [TestMethod]
        public void WhenRegisteringContextualBindingWithLifetimeManager_CorrectLifetimeIsUsed()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TestLogger>(request => typeof (SomeService) == request.TypeToBuild,
                                                        new ContainerControlledLifetimeManager());

            container.RegisterType<ILogger, TestLogger>(request => typeof (AnotherService) == request.TypeToBuild,
                                                        new ContainerControlledLifetimeManager());

            SomeService svc1 = container.Resolve<SomeService>();
            SomeService svc2 = container.Resolve<SomeService>();
            AnotherService svc3 = container.Resolve<AnotherService>();
            AnotherService svc4 = container.Resolve<AnotherService>();

            Assert.IsInstanceOfType(svc1.Logger, typeof(TestLogger));
            Assert.IsInstanceOfType(svc2.Logger, typeof(TestLogger));
            Assert.AreSame(svc1.Logger, svc2.Logger);

            Assert.IsInstanceOfType(svc3.Logger, typeof(TestLogger));
            Assert.IsInstanceOfType(svc4.Logger, typeof(TestLogger));
            Assert.AreSame(svc3.Logger, svc4.Logger);

            Assert.AreNotSame(svc2.Logger, svc4.Logger);
        }
    }
}
