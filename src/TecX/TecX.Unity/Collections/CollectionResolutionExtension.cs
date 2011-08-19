using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace TecX.Unity.Collections
{
    public class CollectionResolutionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            //PreCreation is the wrong stage, should be Creation. But the DynamicMethodConstructorStrategy
            //will receive the request for resolve prior to the CollectionResolutionStrategy and raise
            //an exception for trying to construct an interface. There is no way to insert a strategy at the
            //beginning of a stage as the StagedStrategyChain is append only. Thats the reason why the ArrayResolutionStrategy
            //is added as first strategy in the Creation stage.
            Context.Strategies.AddNew<CollectionResolutionStrategy>(UnityBuildStage.PreCreation);
        }
    }
}
