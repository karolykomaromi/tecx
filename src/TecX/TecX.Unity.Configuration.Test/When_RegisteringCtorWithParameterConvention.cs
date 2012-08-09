namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.Injection;

    [TestClass]
    public class When_RegisteringCtorWithParameterConvention : Given_ContainerAndBuilder
    {
        private HasCtorWithParameterConvention sut;

        protected override void Given()
        {
            base.Given();

            this.builder.For<HasCtorWithParameterConvention>().Use<HasCtorWithParameterConvention>().Ctor(new ConstructorParameter(new Foo()));
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<HasCtorWithParameterConvention>();
        }

        [TestMethod]
        public void Then_AppliesCtorConvention()
        {
            Assert.IsNotNull(this.sut.Foo);
        }
    }
}
