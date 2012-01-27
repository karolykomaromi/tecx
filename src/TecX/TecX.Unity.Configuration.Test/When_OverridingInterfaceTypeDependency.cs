namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_OverridingInterfaceTypeDependency : Given_ContainerAndBuilder
    {
        private MyService service;

        protected override void Given()
        {
            base.Given();

            this.builder.For<IFoo>().Use<Foo>();
            this.builder.For<IFoo>().Add<Bar>().Named("Bar");
            this.builder.For<IMyInterface>().Use<MyClass>();
            this.builder.For<IMyService>().Use<MyService>().Override(new { someFoo = "Bar" });
        }

        protected override void When()
        {
            base.When();

            this.service = this.container.Resolve<IMyService>() as MyService;
        }

        [TestMethod]
        public void Then_ResolvesSpecifiedNamedMapping()
        {
            Assert.IsNotNull(this.service);
            Assert.IsInstanceOfType(this.service.SomeInterface, typeof(MyClass));
            Assert.IsInstanceOfType(this.service.SomeFoo, typeof(Bar));
        }
    }
}
