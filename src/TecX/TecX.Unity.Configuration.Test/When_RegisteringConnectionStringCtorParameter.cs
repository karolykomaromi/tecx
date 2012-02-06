namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class When_RegisteringConnectionStringCtorParameter : Given_ContainerAndBuilder
    {
        private HasCtorWithConnectionString sut;

        protected override void Given()
        {
            base.Given();

            this.builder.For<HasCtorWithConnectionString>().Use<HasCtorWithConnectionString>().Ctor(x => x.With("I'm a ConnectionString"));
        }

        protected override void When()
        {
            base.When();

            this.sut = this.container.Resolve<HasCtorWithConnectionString>();
        }

        [TestMethod]
        public void Then_ConnectionStringIsInjected()
        {
            Assert.AreEqual("I'm a ConnectionString", sut.ConnectionString);
        }
    }

    public class HasCtorWithConnectionString
    {
        public string ConnectionString { get; set; }

        public HasCtorWithConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
