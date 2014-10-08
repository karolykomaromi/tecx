namespace Hydra.Unity.Test.Utility
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture.Kernel;

    public class ContainerSpecimenBuilder : ISpecimenBuilder
    {
        private readonly IUnityContainer container;

        public ContainerSpecimenBuilder(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public object Create(object request, ISpecimenContext context)
        {
            Type type = request as Type;

            if (type == null)
            {
                return new NoSpecimen();
            }

            return this.container.Resolve(type);
        }
    }
}