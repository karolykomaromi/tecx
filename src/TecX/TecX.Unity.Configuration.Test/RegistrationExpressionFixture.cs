using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Expressions;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class RegistrationExpressionFixture
    {
        [TestMethod]
        public void CanRegisterDefaultTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            TypeRegistrationExpression<IMyInterface, MyClass> expression = new TypeRegistrationExpression<IMyInterface, MyClass>();

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterNamedTypeMapping()
        {
            IUnityContainer container = new UnityContainer();

            NamedTypeRegistrationExpression<IMyInterface, MyClass> expression = new NamedTypeRegistrationExpression<IMyInterface, MyClass>();

            expression.Name = "1";

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>("1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClass));
        }

        [TestMethod]
        public void CanRegisterFactoryForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression<IMyInterface, MyClassWithCtorParams>();

            expression.ConstructedBy(c => new MyClassWithCtorParams("1"));

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanSelectCtorForMapping()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression<IMyInterface, MyClassWithCtorParams>();

            expression.SelectConstructor(() => new MyClassWithCtorParams());

            expression.Compile().Configure(container);

            IMyInterface result = container.Resolve<IMyInterface>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MyClassWithCtorParams));
        }

        [TestMethod]
        public void CanRegisterLifetime()
        {
            IUnityContainer container = new UnityContainer();

            var expression = new TypeRegistrationExpression<IMyInterface, MyClass>();

            expression.LifetimeIs(() => new ContainerControlledLifetimeManager());

            expression.Compile().Configure(container);

            var r1 = container.Resolve<IMyInterface>();
            var r2 = container.Resolve<IMyInterface>();

            Assert.AreSame(r1, r2);
        }
    }

    public class MyClassWithCtorParams : IMyInterface
    {
        public string Name { get; set; }

        public MyClassWithCtorParams()
        {
        }

        public MyClassWithCtorParams(string name)
        {
            Name = name;
        }
    }

    public class MyClass : IMyInterface
    {
    }

    public interface IMyInterface
    {
    }
}
