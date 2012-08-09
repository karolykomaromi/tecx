namespace TecX.Unity.Tracking
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class TrackingExtension : UnityContainerExtension, ITracker
    {
        [ThreadStatic]
        private static BuildTreeNode currentBuildNode;

        BuildTreeNode ITracker.CurrentBuildNode
        {
            get
            {
                return currentBuildNode;
            }

            set
            {
                currentBuildNode = value;
            }
        }

        protected override void Initialize()
        {
            this.Context.Strategies.Add(new BuildTreeTrackerStrategy(this), UnityBuildStage.Setup);
            this.Context.Strategies.Add(new CreationTrackerStrategy(this), UnityBuildStage.PreCreation);
            this.Container.RegisterInstance<ITracker>(this);

            // TODO weberse 2012-06-09 need to enhance tracking
            // track original buildkey in first buildstage
            // track mapped key and existing item in pre-creation stage
        }
    }
}
