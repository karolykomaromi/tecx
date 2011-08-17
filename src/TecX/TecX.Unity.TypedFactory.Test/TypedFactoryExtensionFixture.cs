﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.TestTools;
using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_ContainerWithTypedFactoryExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected override void Given()
        {
            container = new UnityContainer();
            container.AddNewExtension<TypedFactoryExtension>();
            container.RegisterType<IFoo, Foo>();
            container.RegisterFactory<IMyFactory>();
        }
    }

    [TestClass]
    public class When_UsingFactoryToCreateInstance : Given_ContainerWithTypedFactoryExtension
    {
        private IFoo _foo;

        protected override void When()
        {
            IMyFactory factory = container.Resolve<IMyFactory>();

            _foo = factory.Create();
        }

        [TestMethod]
        public void Then_AutoGeneratedProxyReturnsInstance()
        {
            Assert.IsNotNull(_foo);
        }
    }
}
