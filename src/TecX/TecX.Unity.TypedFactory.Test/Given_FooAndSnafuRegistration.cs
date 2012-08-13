namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    public abstract class Given_FooAndSnafuRegistration : Given_ContainerWithFactoryRegistration
    {
        protected override void Arrange()
        {
            base.Arrange();

            this.Container.RegisterType<IFoo, Foo>("Foo");
            this.Container.RegisterType<IFoo, Snafu>("Snafu");
        }
    }
}