using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class RegistryFixture
    {
        [TestMethod]
        public void CanUseBasicActionsToAddExtensions()
        {
            Registry registry = new InterceptionRegistry();

            IUnityContainer container = new UnityContainer();

            container.AddExtension(registry);

            Assert.Fail("finish test");
        }
    }

    class InterceptionRegistry : Registry
    {
        public InterceptionRegistry()
        {
            RegisterAction(() => Container.AddNewExtension<Interception>());
        }
    }
}
