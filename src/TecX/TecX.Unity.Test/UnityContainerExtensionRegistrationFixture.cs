using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.AutoRegistration;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class UnityContainerExtensionRegistrationFixture
    {
        [TestMethod]
        public void WhenAddingExtension_CanConfigureItAsExpected()
        {
            IUnityContainer container = new UnityContainer();

            IAutoRegistration registration = container
                .ConfigureAutoRegistration()
                .Include(The.Extension<TestExtension>()
                             .WithConfiguration<ITestExtensionConfig>(c => c.Prop1 = true))
                .ApplyAutoRegistrations();

            TestExtension extension = container.Resolve<TestExtension>();

            Assert.IsTrue(extension.Prop1);
        }
    }
}
