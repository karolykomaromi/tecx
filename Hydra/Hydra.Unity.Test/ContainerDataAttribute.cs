namespace Hydra.Unity.Test
{
    using System;
    using Hydra.TestTools;
    using Hydra.Unity.Tracking;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(
                    new UnityContainer().AddExtension(new DisposableExtension()))))
        {
        }
    }
}