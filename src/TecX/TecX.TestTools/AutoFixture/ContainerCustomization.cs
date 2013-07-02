using Microsoft.Practices.Unity;

using Ploeh.AutoFixture;

using TecX.Common;

namespace TecX.TestTools.AutoFixture
{
    public class ContainerCustomization : ICustomization
    {
        private readonly IUnityContainer container;

        public ContainerCustomization(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");
            this.container = container;
        }

        public void Customize(IFixture fixture)
        {
            Guard.AssertNotNull(fixture, "fixture");

            fixture.ResidueCollectors.Add(new ChildContainerSpecimenBuilder(this.container));
            fixture.ResidueCollectors.Add(new ContainerSpecimenBuilder(this.container));
        }
    }
}