namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DecoraptorProxyBuilderTests
    {
        [Fact]
        public void Should_Create_Lazy_Proxy()
        {
            var generator = new ProxyGenerator();

            Type lazyProxyType = generator.CreateDecoraptorProxy(typeof(IDuck));

            int factoryCalledCounter = 0;

            Func<IDuck> factory = () =>
            {
                factoryCalledCounter++;
                return new Duck();
            };

            IDuck proxy = (IDuck)Activator.CreateInstance(lazyProxyType, factory);

            Assert.Equal(0, factoryCalledCounter);

            Assert.Equal(42, proxy.TheAnswer());

            Assert.Equal(1, factoryCalledCounter);

            Assert.Throws<NotImplementedException>(() => proxy.NotImplementedProperty);

            Assert.Equal(2, factoryCalledCounter);

            Assert.Throws<NotImplementedException>(() => proxy.NotImplementedMethod());

            Assert.Equal(3, factoryCalledCounter);
        }
    }
}