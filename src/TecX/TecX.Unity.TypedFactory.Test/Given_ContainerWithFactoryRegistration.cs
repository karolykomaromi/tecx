namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;
    using TecX.Unity.TypedFactory.Test.TestObjects;

    public abstract class Given_ContainerWithFactoryRegistration : GivenWhenThen
    {
        protected IUnityContainer container;

        protected IFoo foo;

        protected IMyFactory factory;

        protected override void Given()
        {
            this.container = new UnityContainer();
            this.container.RegisterType<IMyFactory>(new TypedFactory());

            this.factory = container.Resolve<IMyFactory>();
        }
    }
}