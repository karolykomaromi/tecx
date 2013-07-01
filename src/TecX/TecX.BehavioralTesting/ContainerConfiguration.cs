namespace TecX.BehavioralTesting
{
    using Microsoft.Practices.Unity;

    public class ContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IFoo, Foo>();
        }
    }
}