namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Hydra.Unity.Collections;
    using Hydra.Unity.Tracking;
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
            this.Container.AddNewExtension<CollectionResolutionExtension>();
            this.Container.AddExtension(new DisposableExtension());

            foreach (Type configurationType in this.assemblies.SelectMany(a => a.GetTypes().Where(this.IsContainerConfiguration)))
            {
                UnityContainerExtension configuration = (UnityContainerExtension)this.Container.Resolve(configurationType);

                this.Container.AddExtension(configuration);
            }
        }

        private bool IsContainerConfiguration(Type t)
        {
            bool isContainerConfiguration = typeof(UnityContainerExtension).IsAssignableFrom(t) &&
                t.Name.EndsWith("Configuration");

            return isContainerConfiguration;
        }
    }
}