using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{

    [TestClass]
    public class OpenGenericFamilyExpressionFixture
    {
        [TestMethod]
        public void CanRegisterMultipleMappingsForOpenGenericTypes()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            var r1 = container.Resolve<ClassThatUsesRepository>();

            Assert.IsInstanceOfType(r1.Repository, typeof(Repository<string>));
        }
    }
}
