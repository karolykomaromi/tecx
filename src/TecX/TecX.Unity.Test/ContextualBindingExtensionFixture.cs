using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.ContextualBinding;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class ContextualBindingExtensionFixture
    {
        [TestMethod]
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

            Assert.IsNotNull(svc2);
            Assert.IsNull(svc2.Logger);
        }

        [TestMethod]
        public void WhenTypeIsRegisteredWithBindingContraint_ResolvesProperly()
        {
            UnityContainer container = new UnityContainer();
            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<ILogger, TestLogger>(
                request => typeof (SomeService) == request.TypeToBuild);

            container.RegisterType<ILogger, TraceLogger>(
                request => typeof (AnotherService) == request.TypeToBuild);

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
                request => typeof (SomeService) == request.TypeToBuild);

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
    }
}
