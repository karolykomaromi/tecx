namespace Hydra.Composition
{
    using System.Runtime.Caching;
    using Microsoft.Practices.Unity;

    public class CachingConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ObjectCache, MemoryCache>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => MemoryCache.Default));
        }
    }
}