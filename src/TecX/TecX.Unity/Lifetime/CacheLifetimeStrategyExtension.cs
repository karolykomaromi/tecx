namespace TecX.Unity.Lifetime
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class CacheLifetimeStrategyExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<CacheLifetimeStrategy>(UnityBuildStage.Lifetime);

            Context.Strategies.AddNew<CacheReleasingLifetimeStrategy>(UnityBuildStage.Lifetime); 
        }
    } 
}
