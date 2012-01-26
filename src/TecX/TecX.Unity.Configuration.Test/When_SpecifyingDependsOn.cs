namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_SpecifyingDependsOn : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            this.builder.For<IFoo>().Use<Foo>();
            this.builder.For<IFoo>().Add<Bar>().Named("Bar");
            this.builder.For<IMyInterface>().Use<MyClass>();
            this.builder.For<IMyService>().Use<MyService>().DependsOn(new { someFoo = "Bar" });
        }

        [TestMethod]
        public void Then_OverridesSpecifiedParameter()
        {
            var svc = this.container.Resolve<IMyService>() as MyService;

            Assert.IsNotNull(svc);
            Assert.IsInstanceOfType(svc.SomeInterface, typeof(MyClass));
            Assert.IsInstanceOfType(svc.SomeFoo, typeof(Bar));
        }
    }

    public class MyService : IMyService
    {
        public IFoo SomeFoo { get; set; }

        public IMyInterface SomeInterface { get; set; }

        public MyService(IFoo someFoo, IMyInterface someInterface)
        {
            SomeFoo = someFoo;
            SomeInterface = someInterface;
        }
    }

    public interface IMyService
    {
    }

    public class Bar : IFoo
    {

    }

    public class Foo : IFoo
    {
    }

    public interface IFoo
    {
    }
}
