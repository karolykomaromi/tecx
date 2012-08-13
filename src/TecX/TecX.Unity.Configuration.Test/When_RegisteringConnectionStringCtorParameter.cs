namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.Injection;

    [TestClass]
    public class When_RegisteringConnectionStringCtorParameter : Given_ContainerAndBuilder
    {
        private HasCtorWithConnectionString sut;

        protected override void Arrange()
        {
            base.Arrange();

            this.builder.For<HasCtorWithConnectionString>().Use<HasCtorWithConnectionString>().Ctor(new ConstructorParameter("I'm a ConnectionString"));
        }

        protected override void Act()
        {
            base.Act();

            this.sut = this.container.Resolve<HasCtorWithConnectionString>();
        }

        [TestMethod]
        public void Then_ConnectionStringIsInjected()
        {
            Assert.AreEqual("I'm a ConnectionString", sut.ConnectionString);
        }
    }
}
