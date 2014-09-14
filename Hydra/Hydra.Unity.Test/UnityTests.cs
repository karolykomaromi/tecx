namespace Hydra.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Xunit;

    public class UnityTests
    {
        [Fact]
        public void Should_Dispose_On_TearDown()
        {
            var container = new UnityContainer().AddNewExtension<TearDownExtension>().RegisterType<DisposeThis>(new TrackingDisposablesTransientLifetimeManager());

            var sut = container.Resolve<DisposeThis>();

            container.Teardown(sut);

            Assert.True(sut.IsDisposed);
        }
    }
}