namespace TecX.BehavioralTesting
{
    using TestObjects;

    using Microsoft.Practices.Unity;

    using Xunit;
    using Xunit.Extensions;

    public class BehavioralTestingFixture
    {
        [Theory, ContainerData]
        public void ShouldUseContainerToResolveParameter(IFoo foo)
        {
            Assert.NotNull(foo);
        }

        [Theory, ContainerData]
        public void ShouldInjectChildContainer(IUnityContainer container)
        {
            Assert.NotNull(container);
            Assert.NotNull(container.Parent);
        }

        [Theory, ContainerData]
        public void DummyTest(OrderFactory factory, Customer customer)
        {
            Assert.NotNull(customer.Name);
            Order order = factory.Build(customer);
            order.Tax.ShouldEqual(10);
        }
    }
}