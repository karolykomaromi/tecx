namespace Hydra.Configuration
{
    using System;
    using System.Linq;
    using Hydra.Unity.Collections;
    using Microsoft.Practices.Unity;

    public class UnityContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<CollectionResolutionExtension>();

            foreach (Type configurationType in this.GetType().Assembly.GetTypes().Where(this.IsContainerConfiguration))
            {
                UnityContainerExtension configuration = (UnityContainerExtension)this.Container.Resolve(configurationType);

                this.Container.AddExtension(configuration);
            }

            ////this.Container.AddNewExtension<ResourcesConfiguration>();

            ////this.Container.AddNewExtension<FubuConfiguration>();

            ////this.Container.AddNewExtension<ErrorHandlingConfiguration>();

            ////this.Container.AddNewExtension<RavenDBConfiguration>();
        }

        private bool IsContainerConfiguration(Type t)
        {
            bool isContainerConfiguration = t != typeof(UnityContainerConfiguration) &&
                (t.Namespace ?? string.Empty).IndexOf("Configuration", StringComparison.Ordinal) > -1 &&
                typeof(UnityContainerExtension).IsAssignableFrom(t) &&
                t.Name.EndsWith("Configuration");

            return isContainerConfiguration;
        }
    }
}