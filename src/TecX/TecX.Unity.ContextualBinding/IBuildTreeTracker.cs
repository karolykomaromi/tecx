using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="IBuildTreeTracker"/>
    ///   interface is used to track build trees as they are created and destroyed.
    /// </summary>
    internal interface IBuildTreeTracker : IBuilderStrategy
    {
        /// <summary>
        /// Assigns the instance to current tree node.
        /// </summary>
        /// <param name="buildKey">
        /// The build key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        void AssignInstanceToCurrentTreeNode(NamedTypeBuildKey buildKey, object instance);

        /// <summary>
        /// Disposes all build trees.
        /// </summary>
        void DisposeAllTrees();

        /// <summary>
        /// Gets the build trees.
        /// </summary>
        /// <value>
        /// The build trees.
        /// </value>
        IList<BuildTreeItemNode> BuildTrees
        {
            get;
        }
    }
}
