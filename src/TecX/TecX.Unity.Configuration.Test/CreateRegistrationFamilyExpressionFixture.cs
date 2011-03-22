using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

        [TestMethod]
        public void HowCanIUseAnExpression()
        {
            var nx = Expression.New(typeof(ContainerControlledLifetimeManager));

            var factory = Expression.Lambda(typeof(Func<LifetimeManager>), nx).Compile();

            var r1 = factory.DynamicInvoke();
            var r2 = factory.DynamicInvoke();

            Assert.AreNotSame(r1, r2);
            Assert.IsInstanceOfType(r1, typeof(ContainerControlledLifetimeManager));
            Assert.IsInstanceOfType(r2, typeof(ContainerControlledLifetimeManager));
        }
    }
}
