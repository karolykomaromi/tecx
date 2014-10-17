namespace Hydra.Infrastructure.Test
{
    using System.Runtime.Caching;
    using Hydra.Infrastructure.I18n;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(
                    new UnityContainer()
                        .RegisterType<ObjectCache>(
                            new ContainerControlledLifetimeManager(), 
                            new InjectionFactory(_ => MemoryCache.Default))
                        .RegisterType<IResxPropertyConvention>(
                        new InjectionFactory(_ => new CompositeConvention(new FindByTypeName(), new FindByTypeFullName()))))))
        {
        }
    }
}
