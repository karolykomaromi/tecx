namespace Hydra
{
    using Hydra.Unity.Collections;
    using Microsoft.Practices.Unity;

    public class UnityContainerConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<CollectionResolutionExtension>();

            this.Container.AddNewExtension<FubuConfiguration>();

            this.Container.AddNewExtension<ErrorHandlingConfiguration>();

            this.Container.AddNewExtension<RavenDBConfiguration>();
        }
    }
}