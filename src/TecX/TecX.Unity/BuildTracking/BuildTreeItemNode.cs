﻿using System;
using System.Collections.ObjectModel;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="BuildTreeItemNode"/>
    ///   class is used to define a build tree item and its children.
    /// </summary>
    internal class BuildTreeItemNode
    {        
        /// <summary>
        /// Gets the build key.
        /// </summary>
        /// <value>
        /// The build key.
        /// </value>
        public NamedTypeBuildKey BuildKey { get; private set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public Collection<BuildTreeItemNode> Children { get; private set; }

        /// <summary>
        /// Gets the item reference.
        /// </summary>
        /// <value>
        /// The item reference.
        /// </value>
        public WeakReference ItemReference { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the node was created by container.
        /// </summary>
        /// <value>
        /// <c>true</c> if the node was created by container; otherwise, <c>false</c>.
        /// </value>
        public bool NodeCreatedByContainer { get; private set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public BuildTreeItemNode Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTreeItemNode"/> class.
        /// </summary>
        /// <param name="buildKey">
        /// The build key.
        /// </param>
        /// <param name="nodeCreatedByContainer">
        /// If set to <c>true</c>, the node was created by container.
        /// </param>
        /// <param name="parentNode">
        /// The parent node.
        /// </param>
        public BuildTreeItemNode(NamedTypeBuildKey buildKey, bool nodeCreatedByContainer,
                                 BuildTreeItemNode parentNode)
        {
            Guard.AssertNotNull(buildKey, "buildKey");

            BuildKey = buildKey;
            NodeCreatedByContainer = nodeCreatedByContainer;
            Parent = parentNode;
            Children = new Collection<BuildTreeItemNode>();
        }

        /// <summary>
        /// Assigns the instance.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <exception cref="ArgumentException">
        /// An instance has already been assign to this node.
        /// </exception>
        public void AssignInstance(object instance)
        {
            if (ItemReference != null)
            {
                throw new ArgumentException("An instance has already been assigned to this node.", "instance");
            }

            ItemReference = new WeakReference(instance);
        }
    }
}