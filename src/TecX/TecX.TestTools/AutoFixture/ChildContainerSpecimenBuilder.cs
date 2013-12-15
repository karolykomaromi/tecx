using System;

using Microsoft.Practices.Unity;

using Ploeh.AutoFixture.Kernel;

using TecX.Common;

namespace TecX.TestTools.AutoFixture
{
    public class ChildContainerSpecimenBuilder : ISpecimenBuilder
    {
        private readonly IUnityContainer container;

        public ChildContainerSpecimenBuilder(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            this.container = container;
        }

        public object Create(object request, ISpecimenContext context)
        {
            Type type = request as Type;

            if (type == null || type != typeof(IUnityContainer))
            {
                return new NoSpecimen();
            }

            return this.container.CreateChildContainer();
        }
    }
}