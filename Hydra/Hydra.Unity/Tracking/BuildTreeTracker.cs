namespace Hydra.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class BuildTreeTracker : BuilderStrategy
    {
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        private static readonly ThreadLocal<BuildTreeItemNode> CurrentNode = new ThreadLocal<BuildTreeItemNode>();

        private readonly List<BuildTreeItemNode> buildTrees = new List<BuildTreeItemNode>();

        public static BuildTreeItemNode CurrentBuildNode
        {
            get
            {
                return CurrentNode.Value;
            }

            set
            {
                CurrentNode.Value = value;
            }
        }

        public virtual IList<BuildTreeItemNode> BuildTrees
        {
            get
            {
                return this.buildTrees;
            }
        }

        public void AssignInstanceToCurrentTreeNode(NamedTypeBuildKey buildKey, object instance)
        {
            if (CurrentBuildNode.BuildKey != buildKey)
            {
                string errorMessageFormat = "Build tree constructed out of order. Build key '{0}' was expected but build key '{1}' was provided.";

                string message = string.Format(CultureInfo.CurrentCulture, errorMessageFormat, CurrentBuildNode.BuildKey, buildKey);

                throw new InvalidOperationException(message);
            }

            CurrentBuildNode.AssignInstance(instance);
        }

        public void DisposeAllTrees()
        {
            using (new UpgradeableReadLock(ReaderWriterLock))
            {
                for (int index = this.BuildTrees.Count - 1; index >= 0; index--)
                {
                    BuildTreeItemNode buildTree = this.BuildTrees[index];

                    this.DisposeTree(buildTree);
                }
            }
        }

        public BuildTreeItemNode GetBuildTreeForInstance(object instance)
        {
            using (new ReadLock(ReaderWriterLock))
            {
                return this.BuildTrees.SingleOrDefault(x => x.Item != null && object.ReferenceEquals(x.Item, instance));
            }
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            // weberse 2014-09-16 We don't track extension lifecycles. They will be disposed when the container is disposed.
            if (typeof(UnityContainerExtension).IsAssignableFrom(context.BuildKey.Type))
            {
                return;
            }

            bool nodeCreatedByContainer = context.Existing == null;

            BuildTreeItemNode newTreeNode;

            if (!nodeCreatedByContainer ||
                context.Lifetime.OfType<ContainerControlledLifetimeManager>().Any() ||
                context.Lifetime.OfType<HierarchicalLifetimeManager>().Any())
            {
                // This object was either not created by the container or is referenced by a LifetimeManager
                // that will take care of disposing it when either this container or the parent container are disposed.
                // Either way it is someone else's problem...
                newTreeNode = new SomeoneElsesProblem(context.BuildKey, CurrentBuildNode);
            }
            else if (typeof(IDisposable).IsAssignableFrom(context.BuildKey.Type))
            {
                // we know this object was created by the container and implements IDisposable so we need to take 
                // care of disposing it
                newTreeNode = new DisposableItemNode(context.BuildKey, CurrentBuildNode);
            }
            else
            {
                // the object is not disposable
                newTreeNode = new NonDisposableItemNode(context.BuildKey, CurrentBuildNode);
            }

            if (CurrentBuildNode != null)
            {
                // This is a child node
                CurrentBuildNode.Children.Add(newTreeNode);
            }

            CurrentBuildNode = newTreeNode;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            // weberse 2014-09-16 We don't track extension lifecycles. They will be disposed when the container is disposed.
            if (typeof(UnityContainerExtension).IsAssignableFrom(context.BuildKey.Type))
            {
                return;
            }

            this.AssignInstanceToCurrentTreeNode(context.BuildKey, context.Existing);

            BuildTreeItemNode parentNode = CurrentBuildNode.Parent;

            if (parentNode == null)
            {
                // This is the end of the creation of the root node
                using (new WriteLock(ReaderWriterLock))
                {
                    this.BuildTrees.Add(CurrentBuildNode);
                }
            }

            // Move the current node back up to the parent
            // If this is the top level node, this will set the current node back to null
            CurrentBuildNode = parentNode;
        }

        public override void PostTearDown(IBuilderContext context)
        {
            BuildTreeItemNode buildTree = this.GetBuildTreeForInstance(context.Existing);

            if (buildTree != null)
            {
                this.DisposeTree(buildTree);
            }

            this.DisposeDeadTrees();
        }

        private void DisposeDeadTrees()
        {
            // Need to enumerate in the reverse order because the trees that are torn down are removed from the set
            using (new UpgradeableReadLock(ReaderWriterLock))
            {
                for (int index = this.BuildTrees.Count - 1; index >= 0; index--)
                {
                    BuildTreeItemNode buildTree = this.BuildTrees[index];

                    if (buildTree.Item == null)
                    {
                        this.DisposeTree(buildTree);
                    }
                }
            }
        }

        private void DisposeTree(BuildTreeItemNode tree)
        {
            var visitor = new BuildTreeDisposer();

            tree.Accept(visitor);

            using (new WriteLock(ReaderWriterLock))
            {
                this.BuildTrees.Remove(tree);
            }
        }
    }
}