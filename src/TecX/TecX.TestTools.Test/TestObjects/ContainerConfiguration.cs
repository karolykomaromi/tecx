using Microsoft.Practices.Unity;

namespace TecX.TestTools.Test.TestObjects
{
    public class ContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IFoo, Foo>();
            this.Container.RegisterType<ITaxCalculator, DefaultTaxCalculator>();
        }
    }
}