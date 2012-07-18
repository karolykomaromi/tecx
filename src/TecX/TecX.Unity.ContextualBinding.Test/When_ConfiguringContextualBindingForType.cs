namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ConfiguringContextualBindingForType : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private IFoo sut;

        protected override void Given()
        {
            base.Given();

            this.builder.For<IFoo>().Use<Foo>().If(request => true);
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<IFoo>();
        }

        [TestMethod]
        public void Then_AppliesPredicate()
        {
            Assert.IsNotNull(this.sut);
        }
    }
}
