namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ConfiguringContextualBindingForType : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private IFoo sut;

        protected override void Arrange()
        {
            base.Arrange();

            this.builder.For<IFoo>().Use<Foo>().When(request => true);
        }

        protected override void Act()
        {
            base.Act();

            this.sut = this.container.Resolve<IFoo>();
        }

        [TestMethod]
        public void Then_AppliesPredicate()
        {
            Assert.IsNotNull(this.sut);
        }
    }
}
