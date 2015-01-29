namespace Hydra.Composition
{
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Configuration;
    using Microsoft.Practices.Unity;

    public class SettingsConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            ISettingsProvider defaultSettings =
                new InMemorySettingsProvider(new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, 1));

            this.Container.RegisterInstance<ISettingsProvider>("default", defaultSettings);

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
                    new ResolvedParameter<ISettingsProvider>("composite"),
                    typeof(ObjectCache)));
        }
    }
}