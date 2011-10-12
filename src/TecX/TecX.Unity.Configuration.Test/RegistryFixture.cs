using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class RegistryFixture
    {
        [TestMethod]
        public void CanUseBasicActionsToAddExtensions()
        {
            var extension = new DummyExtension();

            bool initialized = false;

            extension.Initialized += (s, e) => initialized = true;

            Registry registry = new DummyRegistry(extension);

            IUnityContainer container = new UnityContainer();

            container.AddExtension(registry);

            Assert.IsTrue(initialized);
        }
    }
}
