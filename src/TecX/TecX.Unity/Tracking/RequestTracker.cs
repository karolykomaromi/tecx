namespace TecX.Unity.Tracking
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class RequestTracker : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new SetupTrackerStrategy(), UnityBuildStage.Setup);
            this.Context.Strategies.Add(new RequestTrackerStrategy(), UnityBuildStage.PreCreation);
        }
    }
}