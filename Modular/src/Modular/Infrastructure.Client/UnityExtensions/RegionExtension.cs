namespace Infrastructure.UnityExtensions
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class RegionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<RegionStrategy>(UnityBuildStage.PreCreation);
        }
    }
}
