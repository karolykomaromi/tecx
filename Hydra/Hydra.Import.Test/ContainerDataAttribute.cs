namespace Hydra.Import.Test
{
    using System;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        private static readonly IUnityContainer Container = new UnityContainer().AddNewExtension<NhTestSupportConfiguration>();

        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(Container)))
        {
        }
    }
}
