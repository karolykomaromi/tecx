using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Extensions;
using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class CreateRegistrationFamilyExpressionFixture
    {
        [TestMethod]
        public void CanSetLifetimeForWholeFamily()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                                {
                                    r.For<IMyInterface>().Use<MyClass>().AsSingleton();

                                    r.For<IMyInterface>().LifetimeIs(() => new TransientLifetimeManager());
                                });

            IMyInterface r1 = container.Resolve<IMyInterface>();
            IMyInterface r2 = container.Resolve<IMyInterface>();

            Assert.AreNotSame(r1, r2);

        }
    }
}
