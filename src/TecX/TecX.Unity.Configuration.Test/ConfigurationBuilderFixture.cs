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
    public class ConfigurationBuilderFixture
    {
        [TestMethod]
        public void CanUseBasicActionsToAddExtensions()
        {
            var extension = new DummyExtension();

            bool initialized = false;

            extension.Initialized += (s, e) => initialized = true;

            ConfigurationBuilder builder = new DummyConfigurationBuilder(extension);

            IUnityContainer container = new UnityContainer();

            container.AddExtension(builder);

            Assert.IsTrue(initialized);
        }

        [TestMethod]
        public void CanAddScannerOnSearchForRegistries()
        {
            var container = new UnityContainer();
            container.AddExtension(new ScansForBuilders());

            var mine = container.ResolveAll<IMyInterface>();

            Assert.IsNotNull(mine);
            Assert.IsTrue(mine.Any());
        }
    }
}
