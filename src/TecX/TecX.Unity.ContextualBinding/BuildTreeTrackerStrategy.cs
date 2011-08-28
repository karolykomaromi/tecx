using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="BuildTreeTrackerStrategy"/>
    ///   class is used to track build trees created by the container.
    /// </summary>
    public class BuildTreeTrackerStrategy : BuilderStrategy, IBuildTreeTracker
    {
        private static class Constants
        {
            public static class ErrorMessages
            {
                /// <summary>
                /// Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.
                /// </summary>
                public const string BuildTreeConstructedOutOfOrder = "Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.";
            }
        }

        /// <summary>
        /// Defines the lock used to protect the list of build trees.
        /// </summary>
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Stores the build trees created by the current container.
        /// </summary>
        private readonly List<BuildTreeItemNode> _buildTrees = new List<BuildTreeItemNode>();

        /// <summary>
        /// The _current build node.
        /// </summary>
        [ThreadStatic]
        private static BuildTreeItemNode _currentBuildNode;

        /// <summary>
        /// Assigns the instance to current tree.
        /// </summary>
        /// <param name="buildKey">
        /// The build key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public void AssignInstanceToCurrentTreeNode(NamedTypeBuildKey buildKey, object instance)
        {
            if (CurrentBuildNode.BuildKey != buildKey)
            {
                string message = string.Format(CultureInfo.CurrentCulture, 
                    Constants.ErrorMessages.BuildTreeConstructedOutOfOrder, 
                    CurrentBuildNode.BuildKey, 
                    buildKey);

                throw new InvalidOperationException(message);
            }

            CurrentBuildNode.AssignInstance(instance);
        }

        /// <summary>
        /// Disposes all trees.
        /// </summary>
        public void DisposeAllTrees()
        {
            using (new LockReader(_lock, true))
            {
                for (int index = BuildTrees.Count - 1; index >= 0; index--)
                {
                    BuildTreeItemNode buildTree = BuildTrees[index];

                    DisposeTree(null, buildTree);
                }
            }
        }

        /// <summary>
        /// Gets the build tree for instance.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A <see cref="BuildTreeItemNode"/> instance or <c>null</c> if no item can be found.
        /// </returns>
        public BuildTreeItemNode GetBuildTreeForInstance(object instance)
        {
            using (new LockReader(_lock))
            {
                return BuildTrees.Where(x => x.ItemReference.IsAlive && ReferenceEquals(x.ItemReference.Target, instance)).SingleOrDefault();
            }
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        ///   PostBuildUp method is called when the chain has finished the PreBuildUp
        ///   phase and executes in reverse order from the PreBuildUp calls.
        /// </summary>
        /// <param name="context">
        /// Context of the build operation.
        /// </param>
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context != null)
            {
                AssignInstanceToCurrentTreeNode(context.BuildKey, context.Existing);

                BuildTreeItemNode parentNode = CurrentBuildNode.Parent;

                if (parentNode == null)
                {
                    // This is the end of the creation of the root node
                    using (new LockWriter(_lock))
                    {
                        BuildTrees.Add(CurrentBuildNode);
                    }
                }

                // Move the current node back up to the parent
                // If this is the top level node, this will set the current node back to null
                CurrentBuildNode = parentNode;
            }

            base.PostBuildUp(context);
        }

        /// <summary>
        /// Called during the chain of responsibility for a teardown operation. The
        ///   PostTearDown method is called when the chain has finished the PreTearDown
        ///   phase and executes in reverse order from the PreTearDown calls.
        /// </summary>
        /// <param name="context">
        /// Context of the teardown operation.
        /// </param>
        public override void PostTearDown(IBuilderContext context)
        {
            base.PostTearDown(context);

            // Get the build tree for this item
            if (context != null)
            {
                BuildTreeItemNode buildTree = GetBuildTreeForInstance(context.Existing);

                if (buildTree != null)
                {
                    DisposeTree(context, buildTree);
                }

                DisposeDeadTrees(context);
            }
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        ///   PreBuildUp method is called when the chain is being executed in the
        ///   forward direction.
        /// </summary>
        /// <param name="context">
        /// Context of the build operation.
        /// </param>
        public override void PreBuildUp(IBuilderContext context)
        {
            base.PreBuildUp(context);

            if (context != null)
            {
                bool nodeCreatedByContainer = context.Existing == null;

                BuildTreeItemNode newTreeNode = new BuildTreeItemNode(context.BuildKey, 
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
        /// Disposes the dead trees.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        private void DisposeDeadTrees(IBuilderContext context)
        {
            // Need to enumerate in the reverse order because the trees that are torn down are removed from the set
            using (new LockReader(_lock, true))
            {
                for (int index = BuildTrees.Count - 1; index >= 0; index--)
                {
                    BuildTreeItemNode buildTree = BuildTrees[index];

                    if (buildTree.ItemReference.IsAlive == false)
                    {
                        DisposeTree(context, buildTree);
                    }
                }
            }
        }

        /// <summary>
        /// Disposes the tree.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        private void DisposeTree(IBuilderContext context, BuildTreeItemNode buildTree)
        {
            BuildTreeDisposer.DisposeTree(context, buildTree);

            using (new LockWriter(_lock))
            {
                BuildTrees.Remove(buildTree);
            }
        }

        /// <summary>
        /// Gets the current build node.
        /// </summary>
        /// <value>
        /// The current build node.
        /// </value>
        private static BuildTreeItemNode CurrentBuildNode
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

        /// <summary>
        /// Gets the build trees.
        /// </summary>
        /// <value>
        /// The build trees.
        /// </value>
        public virtual IList<BuildTreeItemNode> BuildTrees
        {
            get
            {
                return _buildTrees;
            }
        }
    }
}
