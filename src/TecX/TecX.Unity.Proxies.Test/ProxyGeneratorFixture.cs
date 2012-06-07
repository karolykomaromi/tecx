namespace TecX.Unity.Proxies.Test
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Unity.Proxies.Test.TestObjects;

    using Xunit;

    public class ProxyGeneratorFixture
    {
        [Fact]
        public void CanRegisterFaultProxy()
        {
            Assert.False(true, "Fix test");

            ////var container = new UnityContainer();

            ////container.AddNewExtension<ProxyGeneratorExtension>();

            ////IProxyGenerator generator = container.Configure<IProxyGenerator>();

            ////Type faultTolerantProxyType = generator.CreateFaultTolerantProxy(typeof(IFooService));

            ////string uniqueName = Guid.NewGuid().ToString();

            ////container.RegisterType<IFooService, FooService>(uniqueName);

            ////container.RegisterType(
            ////    typeof(IFooService),
            ////    faultTolerantProxyType,
            ////    new InjectionConstructor(new ResolvedParameter(typeof(Func<IFooService>), uniqueName)));

            ////IFooService proxy = container.Resolve<IFooService>();

            ////Assert.IsNotType(typeof(FooService), proxy);
            ////Assert.Equal("Foo()", proxy.Foo());
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

            Assert.IsNotType(typeof(FooService), proxy);
            Assert.Equal("Foo()", proxy.Foo());
        }

        [Fact]
        public void CanRegisterLazyProxyUsingNestedClosureSyntax()
        {
            var container = new UnityContainer();

            container.RegisterLazyProxy(
                x =>
                    {
                        x.Contract = typeof(IFooService);
                        x.ServiceImplementation = typeof(FooService);
                    });

            IFooService proxy = container.Resolve<IFooService>();

            Assert.IsNotType(typeof(FooService), proxy);
            Assert.Equal("Foo()", proxy.Foo());
        }
    }
}
