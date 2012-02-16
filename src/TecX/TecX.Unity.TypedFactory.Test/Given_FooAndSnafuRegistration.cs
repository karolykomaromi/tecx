using Microsoft.Practices.Unity;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    public abstract class Given_FooAndSnafuRegistration : Given_ContainerWithFactoryRegistration
    {
        protected override void Given()
        {
            base.Given();

            container.RegisterType<IFoo, Foo>("Foo");
            container.RegisterType<IFoo, Snafu>("Snafu");
        }
    }
}