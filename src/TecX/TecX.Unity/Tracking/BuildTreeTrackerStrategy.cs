namespace TecX.Unity.Tracking
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class BuildTreeTrackerStrategy : BuilderStrategy
    {
        private readonly ITracker tracker;

        public BuildTreeTrackerStrategy(ITracker tracker)
        {
            Guard.AssertNotNull(tracker, "tracker");

            this.tracker = tracker;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            BuildTreeNode newTreeNode = new BuildTreeNode(context.OriginalBuildKey, this.tracker.CurrentBuildNode);

            if (this.tracker.CurrentBuildNode != null)
            {
                // This is a child node
                this.tracker.CurrentBuildNode.Children.Add(newTreeNode);
            }

            this.tracker.CurrentBuildNode = newTreeNode;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            BuildTreeNode parentNode = this.tracker.CurrentBuildNode.Parent;

            // Move the current node back up to the parent
            // If this is the top level node, this will set the current node back to null
            this.tracker.CurrentBuildNode = parentNode;
        }
    }
}