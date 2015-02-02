namespace TecX.BehavioralTesting
{
    using System;

    using Microsoft.Practices.Unity;

    using Ploeh.AutoFixture.Kernel;

    using Common;

    public class ContainerSpecimenBuilder : ISpecimenBuilder
    {
        private readonly IUnityContainer container;

        public ContainerSpecimenBuilder(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

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