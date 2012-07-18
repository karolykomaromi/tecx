namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using TecX.Unity.Factories;
    using TecX.Unity.TypedFactory.Test.TestObjects;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PropertyOverrideFixture
    {
        [TestMethod]
        public void CanInjectProperties()
        {
            var container = new UnityContainer();

            container.AddNewExtension<Interception>();

            container.RegisterType<ICustomerRepositoryFactory>(new TypedFactory());
            container.RegisterType<ICustomerRepository, CustomerRepository>(new InjectionProperty("ConnectionString"));

            var factory = container.Resolve<ICustomerRepositoryFactory>();

            var repository = factory.Create("1");

            Assert.AreEqual("1", repository.ConnectionString);
        }
    }
}
