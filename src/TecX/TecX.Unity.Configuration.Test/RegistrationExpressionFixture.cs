using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Expressions;
using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class RegistrationExpressionFixture
    {
        [TestMethod]
        public void CanRegisterDefaultTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            TypeRegistrationExpression expression = new TypeRegistrationExpression(typeof(IMyInterface), typeof(MyClass));

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterNamedTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new NamedTypeRegistrationExpression(typeof(IMyInterface), typeof(MyClass))
                {
                    Name = "1"
                };

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>("1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterFactoryForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression(typeof(IMyInterface), typeof(MyClassWithCtorParams));

            expression.Factory(c => new MyClassWithCtorParams("1"));

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanSelectCtorForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression(typeof(IMyInterface), typeof(MyClassWithCtorParams));

            expression.Ctor(() => new MyClassWithCtorParams());

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanRegisterLifetime()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression(typeof(IMyInterface), typeof(MyClass));

            expression.LifetimeIs(new ContainerControlledLifetimeManager());

            expression.Compile().Configure(container);

            var r1 = container.Resolve<IMyInterface>();
            var r2 = container.Resolve<IMyInterface>();

            Assert.AreSame(r1, r2);
        }
    }
}
