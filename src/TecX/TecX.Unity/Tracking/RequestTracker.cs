namespace TecX.Unity.Tracking
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class RequestTracker : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new SetupTrackerStrategy(), UnityBuildStage.Setup);

            // although the name indicates otherwise this strategy must be placed in the TypeMapping stage as
            // you can't insert strategies at the front of a stage. If you are using a ContainerControlledLifetimeManager
            // the strategy chain would short-circuit and a strategy placed after the default lifetime strategy would not run
            // and thus not be able to remove the dummy request created during the Setup stage.
            this.Context.Strategies.Add(new LifetimeTrackerStrategy(), UnityBuildStage.TypeMapping);
            this.Context.Strategies.Add(new PreCreationTrackerStrategy(), UnityBuildStage.PreCreation);
        }
    }
}