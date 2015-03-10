namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public class CompositionRoot : UnityContainerExtension
    {
        private readonly HashSet<Assembly> assemblies;

        public CompositionRoot(params Assembly[] assemblies)
        {
            this.assemblies = new HashSet<Assembly>(assemblies ?? new Assembly[0]);
        }

        protected override void Initialize()
        {
            this.Container.AddNewExtension<HydraDefaultExtensions>();

            IEnumerable<Type> configurationTypes = this.assemblies.SelectMany(a => a.GetTypes().Where(IsContainerConfiguration));

            foreach (Type configurationType in configurationTypes)
            {
                UnityContainerExtension configuration = (UnityContainerExtension)this.Container.Resolve(configurationType);

                this.Container.AddExtension(configuration);
            }
        }

        private static bool IsContainerConfiguration(Type type)
        {
            bool isContainerConfiguration = typeof(UnityContainerExtension).IsAssignableFrom(type) &&
                type.Name.EndsWith("Configuration");

            return isContainerConfiguration;
        }
    }
}