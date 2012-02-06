namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.ContextualBinding;

    [TestClass]
    public class When_ConfiguringContextualBindingForType : Given_ContainerAndBuilder
    {
        private IFoo sut;

        protected override void Given()
        {
            base.Given();

            this.container.AddNewExtension<ContextualBindingExtension>();

            this.builder.For<IFoo>().Use<Foo>().If((bindingCtx, builderCtx) => true);
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
