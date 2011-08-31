using Microsoft.Practices.Unity;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_ContainerWithBarRegistration : Given_ContainerWithTypedFactoryExtension
    {
        protected override void Given()
        {
            base.Given();

            container.RegisterType<IFoo, Bar>();
        }
    }
}