namespace TecX.BehavioralTesting
{
    using Microsoft.Practices.Unity;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class ContainerTheoryAttribute : AutoDataAttribute
    {
        public ContainerTheoryAttribute()
            : base(new Fixture().Customize(new ContainerCustomization(new UnityContainer().AddExtension(new ContainerConfiguration()))))
        {
        }
    }
}