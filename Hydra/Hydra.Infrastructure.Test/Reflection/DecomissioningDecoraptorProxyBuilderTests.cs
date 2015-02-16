namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class DecomissioningDecoraptorProxyBuilderTests
    {
        [Fact]
        public void Should_Create_Decomissioning_Decoraptor()
        {
            var generator = new ProxyGenerator();

            Type lazyProxyType = generator.CreateDecomissioningDecoraptorProxy(typeof(IDuck));

            int factoryCalledCounter = 0;
            int releaseCalledCounter = 0;

            Func<IDuck> create = () =>
            {
                factoryCalledCounter++;
                return new Duck();
            };

            Action<IDuck> release = duck =>
            {
                releaseCalledCounter++;
            };

            IDuck proxy = (IDuck)Activator.CreateInstance(lazyProxyType, create, release);

            Assert.Equal(0, factoryCalledCounter);

            Assert.Equal(42, proxy.TheAnswer());

            Assert.Equal(1, factoryCalledCounter);
            Assert.Equal(1, releaseCalledCounter);

            Assert.Throws<NotImplementedException>(() => proxy.NotImplementedProperty);

            Assert.Equal(2, factoryCalledCounter);
            Assert.Equal(2, releaseCalledCounter);

            Assert.Throws<NotImplementedException>(() => proxy.NotImplementedMethod());

            Assert.Equal(3, factoryCalledCounter);
            Assert.Equal(3, releaseCalledCounter);

            proxy.Bar = "1";

            Assert.Equal(4, factoryCalledCounter);
            Assert.Equal(4, releaseCalledCounter);
        }
    }
}