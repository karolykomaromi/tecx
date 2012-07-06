using Microsoft.Practices.Unity;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_ContainerWithFooRegistration : Given_ContainerWithFactoryRegistration
    {
        protected override void Given()
        {
            base.Given();

            this.Container.RegisterType<IFoo, Foo>();
        }
    }
}