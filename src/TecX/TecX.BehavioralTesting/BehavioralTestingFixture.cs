namespace TecX.BehavioralTesting
{
    using Microsoft.Practices.Unity;

    using Xunit;
    using Xunit.Extensions;

    public class BehavioralTestingFixture
    {
        [Theory, ContainerTheory]
        public void ShouldUseContainerToResolveParameter(IFoo foo)
        {
            Assert.NotNull(foo);
        }

        [Theory, ContainerTheory]
        public void ShouldInjectChildContainer(IUnityContainer container)
        {
            Assert.NotNull(container);
            Assert.NotNull(container.Parent);
        }
    }
}