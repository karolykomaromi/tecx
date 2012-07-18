namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Mapping;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class DefaultMappingExtensionFixture
    {
        [TestMethod]
        public void IfNoDefaultMappingIsFoundUseFirstRegisteredNamedMapping()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DefaultMappingExtension>();

            container.RegisterType<IFoo, Foo>("1");

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(Foo));
        }

        [TestMethod]
        public void IfDefaultMappingIsRegisteredDoesntOverride()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DefaultMappingExtension>();

            container.RegisterType<IFoo, Foo>();
            container.RegisterType<IFoo, Snafu>("1");

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(Foo));
        }

        [TestMethod]
        public void IfMultipleNamedMappingsAreRegisteredUsesFirst()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DefaultMappingExtension>();

            container.RegisterType<IFoo, Foo>("1");
            container.RegisterType<IFoo, Snafu>("2");

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(Foo));
        }

        [TestMethod]
        public void MultipleNamedMappingsAndDefaultMappingUsesDefault()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DefaultMappingExtension>();

            container.RegisterType<IFoo, Foo3>("1");
            container.RegisterType<IFoo, Foo>();
            container.RegisterType<IFoo, Snafu>("2");

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
            Assert.IsInstanceOfType(foo, typeof(Foo));
        }
    }
}
