namespace TecX.Unity.Tracking
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    /// <summary>
    /// The <see cref="CreationTrackerStrategy"/>
    ///   class is used to track build trees created by the container.
    /// </summary>
    public class CreationTrackerStrategy : BuilderStrategy
    {
        /// <summary>
        /// Error messages
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class ErrorMessages
        {
            /// <summary>
            /// Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.
            /// </summary>
            public const string BuildTreeConstructedOutOfOrder =
                "Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.";
        }

        private readonly ITracker tracker;

        public CreationTrackerStrategy(ITracker tracker)
        {
            Guard.AssertNotNull(tracker, "tracker");
            this.tracker = tracker;
        }

        /// <inheritdoc/>
        public override void PostBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            this.AssignInstanceToCurrentTreeNode(context.BuildKey, context.Existing);
        }

        /// <inheritdoc/>
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (this.tracker.CurrentBuildNode != null)
            {
                bool nodeCreatedByContainer = context.Existing == null;
                this.tracker.CurrentBuildNode.NodeCreatedByContainer = nodeCreatedByContainer;
                this.tracker.CurrentBuildNode.BuildKey = context.BuildKey;
            }
        }

        /// <summary>
        /// Assigns an instance to the current tree node.
        /// </summary>
        /// <param name="buildKey">
        /// The build key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        private void AssignInstanceToCurrentTreeNode(NamedTypeBuildKey buildKey, object instance)
        {
            if (this.tracker.CurrentBuildNode.BuildKey != buildKey)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    ErrorMessages.BuildTreeConstructedOutOfOrder,
                    this.tracker.CurrentBuildNode.BuildKey,
                    buildKey);

                throw new InvalidOperationException(message);
            }

            this.tracker.CurrentBuildNode.AssignInstance(instance);
        }
    }
}