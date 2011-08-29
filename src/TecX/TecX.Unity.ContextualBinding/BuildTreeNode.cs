using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="BuildTreeNode"/> is used to define a build tree node and its children.
    /// </summary>
    public class BuildTreeNode
    {
        private static class Constants
        {
            public static class ErrorMessages
            {
                /// <summary>
                /// An instance has already been assigned to this node.
                /// </summary>
                public const string InstanceAlreadyAssigned = "An instance has already been assigned to this node.";
            }
        }

        private readonly ICollection<BuildTreeNode> _children;

        #region Properties

        public NamedTypeBuildKey BuildKey { get; private set; }

        public ICollection<BuildTreeNode> Children
        {
            get
            {
                return this._children;
            }
        }

        public WeakReference ItemReference { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the node was created by container.
        /// </summary>
        /// <value>
        /// <c>true</c> if the node was created by container; otherwise, <c>false</c>.
        /// </value>
        public bool NodeCreatedByContainer { get; private set; }

        public BuildTreeNode Parent { get; private set; }

        public BuildTreeNode RootNode
        {
            get
            {
                if (Parent == null)
                {
                    return this;
                }

                BuildTreeNode root = Parent;

                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                return root;
            }
        }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTreeNode"/> class.
        /// </summary>
        /// <param name="buildKey">The build key for the current build operation. </param>
        /// <param name="nodeCreatedByContainer">If set to <c>true</c>, the node was created by the container.</param>
        /// <param name="parentNode">The parent node of this instance in the build tree.</param>
        public BuildTreeNode(NamedTypeBuildKey buildKey,
            bool nodeCreatedByContainer,
            BuildTreeNode parentNode)
        {
            Guard.AssertNotNull(buildKey, "buildKey");

            BuildKey = buildKey;

            NodeCreatedByContainer = nodeCreatedByContainer;

            Parent = parentNode;

            _children = new List<BuildTreeNode>();
        }

        /// <summary>
        /// Assigns an object instance to this node
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentException">
        /// An instance has already been assign to this node.
        /// </exception>
        public void AssignInstance(object instance)
        {
            if (ItemReference != null)
            {
                throw new ArgumentException(Constants.ErrorMessages.InstanceAlreadyAssigned, "instance");
            }

            ItemReference = new WeakReference(instance);
        }
    }
}
