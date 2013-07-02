namespace TecX.TestTools.Test
{
    using Xunit;

    using Microsoft.Practices.Unity;

    using Unity;

    using TestObjects;

    public class AutoMockingExtensionFixture
    {
        [Fact]
        public void CanTurnOnAutoMocking()
        {
            var container = new UnityContainer();

            container.AddNewExtension<AutoMockingExtension>();

            var foo = container.Resolve<IFoo>();

            Assert.NotNull(foo);
        }

        [Fact]
        public void CanConfigureMock()
        {
            var container = new UnityContainer();

            var mock = container.Mock<IFoo>();

            mock.SetupGet(f => f.Bar).Returns("1");

            IFoo foo = container.Resolve<IFoo>();

            Assert.Equal("1", foo.Bar);
        }
    }
}
