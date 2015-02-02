namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.I18n;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        private static readonly IUnityContainer Container = new UnityContainer()
            .RegisterType<ObjectCache>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => MemoryCache.Default))
            .RegisterType<IResXPropertyConvention>(
                new InjectionFactory(_ => new CompositeConvention(new FindByTypeName(), new FindByTypeFullName())))
            .AddNewExtension<NhTestSupportConfiguration>();

        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(Container)))
        {
        }
    }
}
