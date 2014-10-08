namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Injection;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class AllPropertiesFixture
    {
        [TestMethod]
        public void CanInjectAllProperties()
        {
            var container = new UnityContainer();
            container.RegisterType<IBar, Bar2>();
            container.RegisterType<Foo2>(new AllProperties());

            var foo = container.Resolve<Foo2>();

            Assert.IsNotNull(foo.Bar);
            Assert.IsNull(foo.Bar2);
        }
    }
}
