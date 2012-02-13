namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;
    using TecX.Unity.TypedFactory.Test.TestObjects;

    public abstract class Given_ContainerWithTypedFactoryExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected IFoo _foo;

        protected IMyFactory _factory;

        protected override void Given()
        {
            container = new UnityContainer();
            container.RegisterType<IMyFactory>(new TypedFactory());

            _factory = container.Resolve<IMyFactory>();
        }
    }
}