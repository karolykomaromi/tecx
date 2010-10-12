using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="IBuildTreeTrackerStrategy"/>
    ///   interface is used to track build trees as they are created and destroyed.
    /// </summary>
    internal interface IBuildTreeTrackerStrategy : IBuilderStrategy
    {
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
        IEnumerable<BuildTreeItemNode> BuildTrees
        {
            get;
        }
    }
}