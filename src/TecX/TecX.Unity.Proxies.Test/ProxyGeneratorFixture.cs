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

            ProxyGenerator generator = new ProxyGenerator();

            container.AddExtension(generator);

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

        [Fact]
        public void LazyProxyHandlesProperties()
        {
            var container = new UnityContainer();

            container.RegisterLazyProxy(
                x =>
                    {
                        x.Contract = typeof(IHaveProperty);
                        x.ServiceImplementation = typeof(HasProperty);
                    });

            IHaveProperty proxy = container.Resolve<IHaveProperty>();

            Assert.IsNotType(typeof(HasProperty), proxy);
            Assert.Equal("1", proxy.MyProperty);

            proxy.MyProperty = "2";

            Assert.Equal("2", proxy.MyProperty);
        }

        [Fact]
        public void CanGenerateProxyWithoutTargetDummy()
        {
            var container = new UnityContainer();

            var extension = new ProxyGenerator();

            container.AddExtension(extension);

            Type dummyType = extension.CreateInterfaceProxyWithoutTargetDummy(typeof(IFooService));

            object dummy = container.Resolve(dummyType);

            Assert.IsAssignableFrom(typeof(IFooService), dummy);
        }

        [Fact]
        public void ProxyWithoutTargetDummyImplementsProperties()
        {
            var container = new UnityContainer();

            var extension = new ProxyGenerator();

            container.AddExtension(extension);

            Type dummyType = extension.CreateInterfaceProxyWithoutTargetDummy(typeof(IHaveProperty));

            IHaveProperty dummy = (IHaveProperty)container.Resolve(dummyType);

            Assert.Throws<NotImplementedException>(() => dummy.MyProperty);
            Assert.Throws<NotImplementedException>(() => dummy.MyProperty = "1");
        }

        [Fact]
        public void CanInterceptCallsToDummy()
        {
            var container = new UnityContainer();

            container.RegisterInterfaceProxyWithoutTarget(typeof(IFooService), "1", new ContainerControlledLifetimeManager(), new MyCustomBehavior());

            IFooService foo = container.Resolve<IFooService>("1");

            Assert.Equal("Foo()", foo.Foo());
        }

        [Fact]
        public void CanRegisterNullObject()
        {
            var container = new UnityContainer();

            container.RegisterNullObject(typeof(IFoo), null, null);

            IFoo nullObject = container.Resolve<IFoo>();

            Assert.Equal(string.Empty, nullObject.ReturnsString());
        }
    }
}
