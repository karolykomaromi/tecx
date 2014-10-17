namespace Hydra.TestTools
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;

    public class ContainerCustomization : ICustomization
    {
        private readonly IUnityContainer container;

        public ContainerCustomization(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public void Customize(IFixture fixture)
        {
            Contract.Requires(fixture != null);

            fixture.ResidueCollectors.Add(new ChildContainerSpecimenBuilder(this.container));
            fixture.ResidueCollectors.Add(new ContainerSpecimenBuilder(this.container));
        }
    }
}