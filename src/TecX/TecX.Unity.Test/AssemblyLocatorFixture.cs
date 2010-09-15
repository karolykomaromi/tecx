using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.TestTools.Extensions;
using TecX.Unity.Registration;

namespace TecX.Unity.Test
{
    [TestClass]
    public class AssemblyLocatorFixture
    {
        private static string _knownExternalAssembly = "Microsoft.Practices.Unity.Interception";

        private static string _unityLibFolder = string.Empty;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            string[] directories = context.DeploymentDirectory.Split(new[] { @"\" },
                StringSplitOptions.RemoveEmptyEntries);

            string unityLibFolder = string.Empty;

            int index = directories.IndexOf("trunk");

            //access to modified closure is alright here
            (directories.Length - index - 1).Times(() => unityLibFolder += @"..\");

            unityLibFolder += @"lib\Unity2\";

            _unityLibFolder = unityLibFolder;

            _knownExternalAssembly = unityLibFolder + _knownExternalAssembly;
        }

        [TestMethod]
        public void WhenUsingFolderAssemblyLocator_AllUnityAssembliesAreLoadedFromLib()
        {
            FolderAssemblyLocator locator = new FolderAssemblyLocator(_unityLibFolder);

            IEnumerable<Assembly> assemblies = locator.GetAssemblies()
                .OrderBy(a => a.GetName().Name);

            Assert.AreEqual(5, assemblies.Count());

            Assert.AreEqual("Microsoft.Practices.ServiceLocation", assemblies.ElementAt(0).GetName().Name);
            Assert.AreEqual("Microsoft.Practices.Unity", assemblies.ElementAt(1).GetName().Name);
            Assert.AreEqual("Microsoft.Practices.Unity.Configuration", assemblies.ElementAt(2).GetName().Name);
            Assert.AreEqual("Microsoft.Practices.Unity.Interception", assemblies.ElementAt(3).GetName().Name);
            Assert.AreEqual("Microsoft.Practices.Unity.Interception.Configuration", assemblies.ElementAt(4).GetName().Name);
        }
    }
}
