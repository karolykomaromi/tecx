namespace Infrastructure.UnityExtensions
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class ResourceManagerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<ResourceManagerStrategy>(UnityBuildStage.Initialization);
        }
    }
}
