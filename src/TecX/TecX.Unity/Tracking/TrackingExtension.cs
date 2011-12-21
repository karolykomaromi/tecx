namespace TecX.Unity.Tracking
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class TrackingExtension : UnityContainerExtension, ITracker
    {
        private readonly BuildTreeTrackerStrategy tracker;

        public TrackingExtension()
        {
            this.tracker = new BuildTreeTrackerStrategy();
        }

        public BuildTreeTrackerStrategy Tracker
        {
            get
            {
                return this.tracker;
            }
        }

        BuildTreeNode ITracker.CurrentBuildNode
        {
            get
            {
                return this.Tracker.CurrentBuildNode;
            }
        }

        protected override void Initialize()
        {
            this.Context.Strategies.Add(this.Tracker, UnityBuildStage.PreCreation);
            this.Container.RegisterInstance<ITracker>(this);
        }
    }
}
