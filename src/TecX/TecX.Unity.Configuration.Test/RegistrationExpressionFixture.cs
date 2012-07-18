using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    using TecX.Unity.Configuration.Builders;

    [TestClass]
    public class RegistrationExpressionFixture
    {
        [TestMethod]
        public void CanRegisterDefaultTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            TypeRegistrationBuilder builder = new TypeRegistrationBuilder(typeof(IMyInterface), typeof(MyClass));

            builder.Build().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterNamedTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationBuilder(typeof(IMyInterface), typeof(MyClass)).Named("1");

            expression.Build().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>("1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterFactoryForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationBuilder(typeof(IMyInterface), typeof(MyClassWithCtorParams));

            expression.Factory(c => new MyClassWithCtorParams("1"));

            expression.Build().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanSelectCtorForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationBuilder(typeof(IMyInterface), typeof(MyClassWithCtorParams));

            expression.Ctor(() => new MyClassWithCtorParams());

            expression.Build().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanRegisterLifetime()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationBuilder(typeof(IMyInterface), typeof(MyClass));

            expression.LifetimeIs(new ContainerControlledLifetimeManager());

            expression.Build().Configure(container);

            var r1 = container.Resolve<IMyInterface>();
            var r2 = container.Resolve<IMyInterface>();

            Assert.AreSame(r1, r2);
        }
    }
}
