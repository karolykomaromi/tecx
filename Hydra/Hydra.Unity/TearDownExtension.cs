namespace Hydra.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class TearDownExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<TearDownStrategy>(UnityBuildStage.Lifetime);
        }
    }
}