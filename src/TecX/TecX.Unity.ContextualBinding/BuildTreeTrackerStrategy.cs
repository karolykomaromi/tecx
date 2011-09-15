using System;
using System.Globalization;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="BuildTreeTrackerStrategy"/>
    ///   class is used to track build trees created by the container.
    /// </summary>
    public class BuildTreeTrackerStrategy : BuilderStrategy
    {
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
                public const string BuildTreeConstructedOutOfOrder = "Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.";
            }
        }

        /// <summary>
        /// The current build node.
        /// </summary>
        [ThreadStatic]
        private static BuildTreeNode _currentBuildNode;

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
                return _currentBuildNode;
            }

            set
            {
                _currentBuildNode = value;
            }
        }

        /// <inheritdoc/>
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context != null)
            {
                AssignInstanceToCurrentTreeNode(context.BuildKey, context.Existing);

                BuildTreeNode parentNode = CurrentBuildNode.Parent;

                // Move the current node back up to the parent
                // If this is the top level node, this will set the current node back to null
                CurrentBuildNode = parentNode;
            }

            base.PostBuildUp(context);
        }

        /// <inheritdoc/>
        public override void PreBuildUp(IBuilderContext context)
        {
            base.PreBuildUp(context);

            if (context != null)
            {
                bool nodeCreatedByContainer = context.Existing == null;

                BuildTreeNode newTreeNode = new BuildTreeNode(
                    context.BuildKey, 
                    nodeCreatedByContainer, 
                    CurrentBuildNode);

                if (CurrentBuildNode != null)
                {
                    // This is a child node
                    CurrentBuildNode.Children.Add(newTreeNode);
                }

                CurrentBuildNode = newTreeNode;
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
            if (CurrentBuildNode.BuildKey != buildKey)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Constants.ErrorMessages.BuildTreeConstructedOutOfOrder,
                    CurrentBuildNode.BuildKey,
                    buildKey);

                throw new InvalidOperationException(message);
            }

            CurrentBuildNode.AssignInstance(instance);
        }
    }
}
