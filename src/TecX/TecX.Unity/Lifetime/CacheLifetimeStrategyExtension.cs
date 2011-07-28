using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace TecX.Unity.Lifetime
{
    public class CacheLifetimeStrategyExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<CacheLifetimeStrategy>(UnityBuildStage.Lifetime);

            Context.Strategies.AddNew<CacheReleasingLifetimeStrategy>(UnityBuildStage.Lifetime); 
        }
    } 
}
