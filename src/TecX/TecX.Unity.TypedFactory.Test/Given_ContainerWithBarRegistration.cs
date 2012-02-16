using Microsoft.Practices.Unity;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_ContainerWithBarRegistration : Given_ContainerWithFactoryRegistration
    {
        protected override void Given()
        {
            base.Given();

            this.container.RegisterType<IFoo, Bar>();
        }
    }
}