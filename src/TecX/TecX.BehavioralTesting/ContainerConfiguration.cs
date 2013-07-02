namespace TecX.BehavioralTesting
{
    using Microsoft.Practices.Unity;

    using TestObjects;

    public class ContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IFoo, Foo>();
            this.Container.RegisterType<ITaxCalculator, DefaultTaxCalculator>();
        }
    }
}