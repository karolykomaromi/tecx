namespace Hydra.Unity.Collections
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class CollectionSupportExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ResolverOverrideExtractor, CoupledToImplementationDetailsOverrideExtractor>();

            // PreCreation is the wrong stage, should be Creation. But the DynamicMethodConstructorStrategy
            // will receive the request for resolve prior to the CollectionSupportStrategy and raise
            // an exception for trying to construct an interface. There is no way to insert a strategy at the
            // beginning of a stage as the StagedStrategyChain is append only. Thats the reason why the ArrayResolutionStrategy
            // is added as first strategy in the Creation stage.
            this.Context.Strategies.AddNew<CollectionSupportStrategy>(UnityBuildStage.PreCreation);

            this.Context.ChildContainerCreated += OnChildContainerCreated;
        }

        private static void OnChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddExtension(new CollectionSupportExtension());
        }
    }
}
