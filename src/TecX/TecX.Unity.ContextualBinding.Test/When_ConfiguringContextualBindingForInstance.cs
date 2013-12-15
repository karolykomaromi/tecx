namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ConfiguringContextualBindingForInstance : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private IFoo sut;

        private object instance;

        protected override void Arrange()
        {
            base.Arrange();

            this.instance = new Foo();

            this.builder.For<IFoo>().Use(this.instance).When(request => true);
        }

        protected override void Act()
        {
            base.Act();

            this.sut = this.container.Resolve<IFoo>();
        }

        [TestMethod]
        public void Then_AppliesPredicate()
        {
            Assert.AreSame(this.instance, this.sut);
        }
    }
}