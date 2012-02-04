namespace TecX.Unity.Configuration.Test
{
    using System.IO;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Plugin;
    using TecX.Unity.Configuration.TestObjects;

    [TestClass]
    public class PluginFinderFixture
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanLoadAdditionalBuilders()
        {
            string currentFolder = this.TestContext.TestDir;

            var parent = new ConfigurationBuilder();

            var finder = new PluginFinder(parent, currentFolder);

            string assemblyFile = Path.Combine(currentFolder, "TecX.Unity.Configuration.TestObjects.dll");

            finder.ImportBuildersFromNewAssembly(assemblyFile);

            var container = new UnityContainer();

            container.AddExtension(parent);

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
        }
    }
}
