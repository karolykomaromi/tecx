namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    /// <summary>
    /// The <see cref="BuildTreeTrackerStrategy"/>
    ///   class is used to track build trees created by the container.
    /// </summary>
    public class BuildTreeTrackerStrategy : BuilderStrategy
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>
            /// Error messages
            /// </summary>
            public static class ErrorMessages
            {
                /// <summary>
                /// Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.
                /// </summary>
                public const string BuildTreeConstructedOutOfOrder =
                    "Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.";
            }
        }

        /// <summary>
        /// The current build node.
        /// </summary>
        [ThreadStatic]
        private static BuildTreeNode currentBuildNode;

        /// <summary>
        /// Gets or sets the current build node.
        /// </summary>
        /// <value>
        /// The current build node.
        /// </value>
        public BuildTreeNode CurrentBuildNode
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

        /// <inheritdoc/>
        public override void PostBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(() => context);

            this.AssignInstanceToCurrentTreeNode(context.BuildKey, context.Existing);

            BuildTreeNode parentNode = this.CurrentBuildNode.Parent;

            // Move the current node back up to the parent
            // If this is the top level node, this will set the current node back to null
            this.CurrentBuildNode = parentNode;
        }

        /// <inheritdoc/>
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(() => context);

            bool nodeCreatedByContainer = context.Existing == null;

            BuildTreeNode newTreeNode = new BuildTreeNode(
                context.BuildKey, nodeCreatedByContainer, this.CurrentBuildNode);

            if (this.CurrentBuildNode != null)
            {
                // This is a child node
                this.CurrentBuildNode.Children.Add(newTreeNode);
            }

            this.CurrentBuildNode = newTreeNode;
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
            if (this.CurrentBuildNode.BuildKey != buildKey)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Constants.ErrorMessages.BuildTreeConstructedOutOfOrder,
                    this.CurrentBuildNode.BuildKey,
                    buildKey);

                throw new InvalidOperationException(message);
            }

            this.CurrentBuildNode.AssignInstance(instance);
        }
    }
}