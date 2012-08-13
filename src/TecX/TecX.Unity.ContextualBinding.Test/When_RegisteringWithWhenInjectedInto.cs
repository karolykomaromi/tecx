namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringWithWhenInjectedInto : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private Parent parent;

        protected override void Arrange()
        {
            base.Arrange();

            this.builder.For<IFoo>().Use<Foo>().WhenInjectedInto<Parent>();
        }

        protected override void Act()
        {
            base.Act();

            this.parent = this.container.Resolve<Parent>();
        }

        [TestMethod]
        public void Then_UsesContextualBindingOnMatch()
        {
            Assert.IsNotNull(this.parent);
            Assert.IsInstanceOfType(this.parent.Foo, typeof(Foo));
        }
    }

    public class Parent
    {
        public IFoo Foo { get; set; }

        public Parent(IFoo foo)
        {
            Foo = foo;
        }
    }
}
