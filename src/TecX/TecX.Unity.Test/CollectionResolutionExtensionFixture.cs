namespace TecX.Unity.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Collections;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
	public class CollectionResolutionExtensionFixture
	{
        [TestMethod]
        public void CanResolveListWithOverrides()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<CollectionResolutionExtension>();

            container.RegisterType<IFoo, Foo>("Foo");
            container.RegisterType<IFoo, Bar>("Bar");

            var list = container.Resolve<IList<IFoo>>(new ParameterOverride("name", "1"));

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list.OfType<Foo>().Count());
            Assert.AreEqual(1, list.OfType<Bar>().Count());
            Assert.AreEqual("1", list.OfType<Bar>().Single().Name);
        }
	}
}
