using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Test.TestObjects;
    using TecX.Unity.TestSupport;

    [TestClass]
    public class WasResolvedFixture
    {
        [TestMethod]
        public void CanFindOutWetherItemWasResolved()
        {
            var extension = new WasResolvedExtension();

            var container = new UnityContainer();

            container.AddExtension(extension);

            container.RegisterType<IFoo, Foo>("1");

            var foo = container.Resolve<IFoo>("1");

            Assert.IsNotNull(foo);
            Assert.IsTrue(extension.WasResolved<IFoo>("1"));
        }
    }
}
