namespace TecX.TestTools.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class AutoMockingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new AutoMockingBuilderStrategy(this.Container);

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}
