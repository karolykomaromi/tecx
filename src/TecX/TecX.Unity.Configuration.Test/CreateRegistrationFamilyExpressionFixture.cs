using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Expressions;
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

            RegistrationGraph graph = new RegistrationGraph();

            graph.Configure(x =>
                                {
                                    var family = x.For<IMyInterface>().Use<MyClass>().AsSingleton();

                                    family.LifetimeIs(() => new TransientLifetimeManager());
                                });

            graph.Configure(container);

            IMyInterface r1 = container.Resolve<IMyInterface>();
            IMyInterface r2 = container.Resolve<IMyInterface>();

            Assert.AreNotSame(r1, r2);
        }
    }
}
