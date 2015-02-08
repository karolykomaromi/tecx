namespace Hydra.Composition
{
    using System.Linq;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Configuration;
    using Microsoft.Practices.Unity;

    public class SettingsConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterInstance<ISettingsProvider>("default", new InMemorySettingsProvider(DefaultSettings.All().ToArray()));

            this.Container.RegisterType<ISettingsProvider, NhSettingsProvider>("database");

            this.Container.RegisterType<ISettingsProvider, WebConfigSettingsProvider>("file");

            this.Container.RegisterType<ISettingsProvider, CompositeSettingsProvider>(
                "composite",
                new InjectionFactory(
                    c => new CompositeSettingsProvider(
                        c.Resolve<ISettingsProvider>("default"),
                        c.Resolve<ISettingsProvider>("database"),
                        c.Resolve<ISettingsProvider>("file"))));

            this.Container.RegisterType<ISettingsProvider, CachingSettingsProvider>(
                new InjectionConstructor(
                    typeof(ObjectCache),
                    new ResolvedParameter<ISettingsProvider>("composite")));
        }
    }
}