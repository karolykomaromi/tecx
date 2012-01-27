namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_OverridingWithValue : Given_ContainerAndBuilder
    {
        private ClassWithPredefinedObjectCtorParameter sut;

        protected override void Given()
        {
            base.Given();

            this.builder.For<ClassWithPredefinedObjectCtorParameter>().Use<ClassWithPredefinedObjectCtorParameter>().
                Override(new { foo = new Bar() });
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<ClassWithPredefinedObjectCtorParameter>();
        }

        [TestMethod]
        public void Then_UsesSpecifiedValue()
        {
            Assert.IsInstanceOfType(this.sut.Foo, typeof(Bar));
        }
    }
}