namespace Hydra.Unity
{
    using Hydra.Unity.Collections;
    using Hydra.Unity.Decoration;
    using Hydra.Unity.Tracking;
    using Microsoft.Practices.Unity;

    public class HydraDefaultExtensions : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<CollectionResolutionExtension>();

            this.Container.AddNewExtension<DisposableExtension>();

            this.Container.AddNewExtension<DecoratorExtension>();
        }
    }
}
