namespace TecX.TestTools.Test
{
    using Microsoft.Practices.Unity;
    using TecX.TestTools.AutoFixture;
    using TecX.TestTools.Test.TestObjects;
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
            Assert.NotNull(customer.FirstName);
            Order order = factory.Build(customer);
            Assert.Equal(order.Tax, 10.0);
        }
    }
}
