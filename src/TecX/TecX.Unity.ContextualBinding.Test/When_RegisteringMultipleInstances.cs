namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringMultipleInstances : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private IFoo sut;

        private Foo foo;

        private Bar bar;

        protected override void Arrange()
        {
            base.Arrange();

            this.foo = new Foo();

            this.bar = new Bar();

            this.builder.For<IFoo>().Use(this.bar).When(request => true);

            this.builder.For<IFoo>().Use(this.foo);
        }

        protected override void Act()
        {
            base.Act();

            this.sut = container.Resolve<IFoo>();
        }

        [TestMethod]
        public void Then_RegisteringSecondInstanceDoesNotOverwriteFirstInstance()
        {
            Assert.AreSame(this.bar, this.sut);
        }
    }

    public class Bar : IFoo
    {
    }
}
