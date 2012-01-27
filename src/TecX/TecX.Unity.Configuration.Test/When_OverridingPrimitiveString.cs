namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_OverridingPrimitiveString : Given_ContainerAndBuilder
    {
        private ClassWithStringCtorParameter sut;

        protected override void Given()
        {
            base.Given();

            this.builder.For<ClassWithStringCtorParameter>().Use<ClassWithStringCtorParameter>().Override(
                new { someString = "1" });
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<ClassWithStringCtorParameter>();
        }

        [TestMethod]
        public void Then_InjectsString()
        {
            Assert.AreEqual("1", this.sut.SomeString);
        }
    }
}