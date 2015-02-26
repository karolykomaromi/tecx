namespace Hydra.Test
{
    using System;
    using Hydra.Composition;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(
                    new UnityContainer().AddExtension(new MediatorConfiguration()))))
        {
        }
    }
}
