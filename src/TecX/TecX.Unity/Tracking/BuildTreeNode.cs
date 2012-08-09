namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    /// <summary>
    /// The <see cref="BuildTreeNode"/> is used to define a build tree node and its children.
    /// </summary>
    [DebuggerDisplay("Type:{BuildKey.Type} Name:{BuildKey.Name}")]
    public class BuildTreeNode
    {
        /// <summary>
        /// Error messages
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed. Suppression is OK here.")]
        private static class ErrorMessages
        {
            /// <summary>
            /// An instance has already been assigned to this node.
            /// </summary>
            public const string InstanceAlreadyAssigned = "An instance has already been assigned to this node.";
        }

        private readonly ICollection<BuildTreeNode> children;

        public BuildTreeNode(NamedTypeBuildKey originalBuildKey, BuildTreeNode parentNode)
        {
            Guard.AssertNotNull(originalBuildKey, "originalBuildKey");

            this.OriginalBuildKey = originalBuildKey;
            this.Parent = parentNode;
            this.children = new List<BuildTreeNode>();
        }

        public NamedTypeBuildKey BuildKey { get; set; }

        public NamedTypeBuildKey OriginalBuildKey { get; private set; }

        public ICollection<BuildTreeNode> Children
        {
            get
            {
                return this.children;
            }
        }

        public WeakReference ItemReference { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the node was created by container.
        /// </summary>
        /// <value>
        /// <c>true</c> if the node was created by container; otherwise, <c>false</c>.
        /// </value>
        public bool NodeCreatedByContainer { get; set; }

        public BuildTreeNode Parent { get; private set; }

        public BuildTreeNode RootNode
        {
            get
            {
                if (this.Parent == null)
                {
                    return this;
                }

                BuildTreeNode root = this.Parent;

                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                return root;
            }
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
            if (this.ItemReference != null)
            {
                throw new ArgumentException(ErrorMessages.InstanceAlreadyAssigned, "instance");
            }

            this.ItemReference = new WeakReference(instance);
        }

        /// <summary>
        /// Implements the visitor pattern.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(IBuildTreeNodeVisitor visitor)
        {
            Guard.AssertNotNull(visitor, "visitor");

            visitor.Visit(this);

            foreach (var child in this.Children)
            {
                child.Accept(visitor);
            }
        }
    }
}