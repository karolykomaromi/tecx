﻿namespace TecX.Unity.Proxies.Test
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Unity.Proxies.Test.TestObjects;

    using Xunit;

    public class FaultTolerantProxyGeneratorFixture
    {
        [Fact]
        public void CanRegisterFaultProxy()
        {
            var container = new UnityContainer();

            container.AddNewExtension<ProxyGeneratorExtension>();

            IProxyGenerator generator = container.Configure<IProxyGenerator>();

            Type faultTolerantProxyType = generator.CreateFaultTolerantProxy(typeof(IFooService));

            string uniqueName = Guid.NewGuid().ToString();

            container.RegisterType<IFooService, FooService>(uniqueName);

            container.RegisterType(
                typeof(IFooService),
                faultTolerantProxyType,
                new InjectionConstructor(new ResolvedParameter(typeof(Func<IFooService>), uniqueName)));

            IFooService proxy = container.Resolve<IFooService>();

            Assert.Equal("Foo()", proxy.Foo());
        }

        [Fact]
        public void CanRegisterLazyProxy()
        {
            var container = new UnityContainer();

            container.AddNewExtension<ProxyGeneratorExtension>();

            IProxyGenerator generator = container.Configure<IProxyGenerator>();

            Type lazyProxyType = generator.CreateLazyInstantiationProxy(typeof(IFooService));

            string uniqueName = Guid.NewGuid().ToString();

            container.RegisterType<IFooService, FooService>(uniqueName);

            container.RegisterType(
                typeof(IFooService),
                lazyProxyType,
                new InjectionConstructor(new ResolvedParameter(typeof(Func<IFooService>), uniqueName)));

            IFooService proxy = container.Resolve<IFooService>();

            Assert.Equal("Foo()", proxy.Foo());
        }
    }
}
