using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class PropertyOverrideFixture
    {
        [TestMethod]
        public void CanInjectProperties()
        {
            var container = new UnityContainer();

            container.RegisterType<ICustomerRepositoryFactory>(new TypedFactory());
            container.RegisterType<ICustomerRepository, CustomerRepository>(new InjectionProperty("ConnectionString"));

            var factory = container.Resolve<ICustomerRepositoryFactory>();

            var repository = factory.Create("1");

            Assert.AreEqual("1", repository.ConnectionString);
        }
    }
}
