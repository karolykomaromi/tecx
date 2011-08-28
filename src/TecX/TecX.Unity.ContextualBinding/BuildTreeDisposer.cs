﻿using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="BuildTreeDisposer"/>
    ///   class is used to dispose build trees.
    /// </summary>
    public static class BuildTreeDisposer
    {
        /// <summary>
        /// Disposes the tree.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="buildTree">
        /// The build tree.
        /// </param>
        public static void DisposeTree(IBuilderContext context, BuildTreeItemNode buildTree)
        {
            TeardownTreeNode(context, buildTree);
        }

        /// <summary>
        /// Determines whether this instance [can teardown instance] the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="instanceReference">
        /// The instance reference.
        /// </param>
        /// <returns>
        /// <c>true</c> if this instance [can teardown instance] the specified context; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanTeardownInstance(IBuilderContext context, WeakReference instanceReference)
        {
            return CanTeardownInstance(context, instanceReference.Target);
        }

        /// <summary>
        /// Determines whether this instance [can teardown instance] the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if this instance [can teardown instance] the specified context; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanTeardownInstance(IBuilderContext context, object instance)
        {
            if (InstanceExistsInLifetimeManager(context, instance))
            {
                // This instance is still stored in the Unity container for future reference
                return false;
            }

            return true;
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="instanceReference">The instance reference.</param>
        private static void DisposeInstance(WeakReference instanceReference)
        {
            if (instanceReference.IsAlive)
            {
                DisposeInstance(instanceReference.Target);
            }
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        private static void DisposeInstance(object instance)
        {
            IDisposable disposable = instance as IDisposable;

            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    Debug.WriteLine("object was already disposed");
                }
            }
        }

        /// <summary>
        /// Instances the exists in lifetime manager.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> instance.
        /// </returns>
        private static bool InstanceExistsInLifetimeManager(IBuilderContext context, object instance)
        {
            if (context == null)
            {
                return false;
            }

            return context.Lifetime.OfType<ILifetimePolicy>().Any(lifetimeManager => ReferenceEquals(lifetimeManager.GetValue(), instance));
        }

        /// <summary>
        /// Teardowns the tree node.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="treeNode">
        /// The tree node.
        /// </param>
        private static void TeardownTreeNode(IBuilderContext context, BuildTreeItemNode treeNode)
        {
            // If the parent node can't be torn down then neither can any of the children
            if (CanTeardownInstance(context, treeNode.ItemReference) == false)
            {
                return;
            }

            // Only nodes created by the unit container will be disposed
            if (treeNode.NodeCreatedByContainer)
            {
                DisposeInstance(treeNode.ItemReference);
            }

            // Recursively call through the child nodes
            for (int index = 0; index < treeNode.Children.Count; index++)
            {
                BuildTreeItemNode child = treeNode.Children[index];

                TeardownTreeNode(context, child);
            }

            return;
        }
    }
}
