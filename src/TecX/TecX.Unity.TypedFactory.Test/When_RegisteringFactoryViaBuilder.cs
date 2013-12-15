namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class When_RegisteringFactoryViaBuilder : Given_ContainerAndBuilder
    {
        private IMyFactory sut;

        protected override void Arrange()
        {
            base.Arrange();

            this.builder.For<IMyFactory>().AsFactory();
            this.builder.For<IFoo>().Use<Foo>();
        }

        protected override void Act()
        {
            base.Act();

            this.sut = this.container.Resolve<IMyFactory>();
        }

        [TestMethod]
        public void Then_CanResolveInstancesFromFactory()
        {
            IFoo foo = this.sut.Create();

            Assert.IsNotNull(foo);
        }
    }
}