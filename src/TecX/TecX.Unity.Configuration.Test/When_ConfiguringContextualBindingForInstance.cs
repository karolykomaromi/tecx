namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.ContextualBinding;

    [TestClass]
    public class When_ConfiguringContextualBindingForInstance : Given_ContainerAndBuilder
    {
        private IFoo sut;

        private object instance;

        protected override void Given()
        {
            base.Given();

            this.container.AddNewExtension<ContextualBindingExtension>();

            this.instance = new Foo();

            this.builder.For<IFoo>().Use(this.instance).If((bindingCtx, builderCtx) => true);
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<IFoo>();
        }

        [TestMethod]
        public void Then_AppliesPredicate()
        {
            Assert.AreSame(this.instance, this.sut);
        }
    }
}