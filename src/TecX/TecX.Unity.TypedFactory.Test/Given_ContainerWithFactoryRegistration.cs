namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using TecX.TestTools;
    using TecX.Unity.TypedFactory.Test.TestObjects;

    public abstract class Given_ContainerWithFactoryRegistration : GivenWhenThen
    {
        protected IUnityContainer Container { get; set; }

        protected IFoo Foo { get; set; }

        protected IMyFactory Factory { get; set; }

        protected override void Given()
        {
            this.Container = new UnityContainer();

            this.Container.AddNewExtension<Interception>();

            this.Container.RegisterType<IMyFactory>(new TypedFactory());

            this.Factory = this.Container.Resolve<IMyFactory>();
        }
    }
}