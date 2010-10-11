using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using TecX.Common;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="BuildTreeStore"/>
    ///   class is used to store build trees.
    /// </summary>
    internal class BuildTreeStore : IBuildTreeStore
    {
        /// <summary>
        /// Defines the lock used to protect the list of build trees.
        /// </summary>
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Stores the build trees created by the current container.
        /// </summary>
        private readonly List<BuildTreeItemNode> _buildTrees;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTreeStore"/> class
        /// </summary>
        public BuildTreeStore()
        {
            _buildTrees = new List<BuildTreeItemNode>();   
        }

        /// <summary>
        /// Adds the specified build tree.
        /// </summary>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        public void Add(BuildTreeItemNode buildTree)
        {
            Guard.AssertNotNull(buildTree, "buildTree");
            Guard.AssertNotNull(buildTree.ItemReference, "buildTree.ItemReference");
            Guard.AssertCondition(buildTree.ItemReference.IsAlive, buildTree.ItemReference.IsAlive, "buildTree.ItemReference.IsAlive");

            using (new LockWriter(Lock))
            {
                BuildTreeItemNode existingTree = GetExistingBuildTreeInternal(buildTree.ItemReference.Target);

                if (existingTree == null)
                {
                    _buildTrees.Add(buildTree);
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
        /// A <see cref="BuildTreeItemNode"/> instance.
        /// </returns>
        public BuildTreeItemNode GetBuildTreeForInstance(object instance)
        {
            using (new LockReader(Lock))
            {
                return GetExistingBuildTreeInternal(instance);
            }
        }

        /// <summary>
        /// Gets the build trees.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> instance.
        /// </returns>
        public IEnumerable<BuildTreeItemNode> GetBuildTrees()
        {
            using (new LockReader(Lock))
            {
                List<BuildTreeItemNode> treeCopy = new List<BuildTreeItemNode>(_buildTrees);
                
                return new ReadOnlyCollection<BuildTreeItemNode>(treeCopy);
            }
        }

        /// <summary>
        /// Removes the specified build tree.
        /// </summary>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        public void Remove(BuildTreeItemNode buildTree)
        {
            using (new LockWriter(Lock))
            {
                _buildTrees.Remove(buildTree);
            }
        }

        /// <summary>
        /// Gets the existing build tree internal.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A <see cref="BuildTreeItemNode"/> instance.
        /// </returns>
        private BuildTreeItemNode GetExistingBuildTreeInternal(object instance)
        {
            return _buildTrees
                .Where(x => x.ItemReference.IsAlive && 
                    ReferenceEquals(x.ItemReference.Target, instance))
                .SingleOrDefault();
        }
    }
}