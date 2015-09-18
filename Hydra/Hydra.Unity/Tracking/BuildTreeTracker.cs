namespace Hydra.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Hydra.Infrastructure;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    [DebuggerDisplay("Tag={Tag} Count={BuildTrees.Count}")]
    public class BuildTreeTracker : BuilderStrategy, IDisposable
    {
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        private readonly ThreadLocal<BuildTreeItemNode> currentNode = new ThreadLocal<BuildTreeItemNode>();

        private readonly List<BuildTreeItemNode> buildTrees = new List<BuildTreeItemNode>();

        private readonly string tag;

        public BuildTreeTracker()
        {
            this.tag = Guid.NewGuid().ToString(FormatStrings.Guid.Hyphens);
        }

        public BuildTreeItemNode CurrentBuildNode
        {
            get { return this.currentNode.Value; }

            set { this.currentNode.Value = value; }
        }

        public string Tag
        {
            get { return this.tag; }
        }

        public IList<BuildTreeItemNode> BuildTrees
        {
            get
            {
                return this.buildTrees;
            }
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            // weberse 2014-09-16 We don't track extension lifecycles. They will be disposed when the container is disposed.
            if (WeAreResovlingContainerExtension(context))
            {
                return;
            }

            // If you use child containers this strategy might be called by all containers in the hierarchy.
            // The last strategy called will be the one in the child container that was actually asked to Resolve the object.
            // This policy is used to track which strategy instances where tracking the creation of this object. That information
            // will be used to avoid tracking by a strategy in the wrong container.
            ICurrentBuildNodePolicy policy = GetCurrentBuildNodePolicy(context);

            this.MarkThatWeTookPartInResolution(policy);

            BuildTreeItemNode newTreeNode;

            if (!IsObjectCreatedByContainer(context) || HasNonTransientLifetime(context))
            {
                // This object was either not created by the container or is referenced by a LifetimeManager
                // that will take care of disposing it when either this container or the parent container are disposed or whose
                // lifetime is externally controlled. Either way it is someone else's problem...
                newTreeNode = new SomeoneElsesProblem(context.BuildKey, this.CurrentBuildNode);
            }
            else if (IsObjectDisposable(context))
            {
                // we know this object was created by the container and implements IDisposable so we need to take 
                // care of disposing it
                newTreeNode = new DisposableItemNode(context.BuildKey, this.CurrentBuildNode);
            }
            else
            {
                // the object is not disposable
                newTreeNode = new NonDisposableItemNode(context.BuildKey, this.CurrentBuildNode);
            }

            this.AddToBuildTreeIfNecessary(newTreeNode);

            this.CurrentBuildNode = newTreeNode;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            // We don't track extension lifecycles. They will be disposed when the container is disposed.
            if (WeAreResolvingContainerExtension(context))
            {
                return;
            }

            // If you use child containers this strategy might be called by all containers in the hierarchy.
            // The last strategy called will be the one in the child container that was actually asked to Resolve the object.
            // Every other strategy has no business in tracking this object so we short circuit immediately.
            ICurrentBuildNodePolicy policy = context.Policies.Get<ICurrentBuildNodePolicy>(context.BuildKey);

            if (this.WeAreNotResponsibleForDisposal(policy))
            {
                this.CurrentBuildNode = null;

                return;
            }

            this.AssertBuildTreeIsNotConstructedOutOfOrder(context);

            this.CurrentBuildNode.AssignInstance(context.Existing);

            BuildTreeItemNode parentNode = this.CurrentBuildNode.Parent;

            this.CreateNewBuildTreeIfNecessary(parentNode);

            // Move the current node back up to the parent
            // If this is the top level node, this will set the current node back to null
            this.CurrentBuildNode = parentNode;
        }

        public override void PostTearDown(IBuilderContext context)
        {
            BuildTreeItemNode buildTree = this.GetBuildTreeForInstance(context.Existing);

            if (buildTree != null)
            {
                this.DisposeItemsAndRemoveTree(buildTree);
            }

            this.PurgeDeadTrees();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static bool WeAreResolvingContainerExtension(IBuilderContext context)
        {
            return typeof(UnityContainerExtension).IsAssignableFrom(context.BuildKey.Type);
        }

        private static bool WeNeedNewTree(BuildTreeItemNode parentNode)
        {
            return parentNode == null;
        }
        
        private static bool HasNonTransientLifetime(IBuilderContext context)
        {
            return context.Lifetime != null && context.Lifetime.OfType<LifetimeManager>().Any(lm => !(lm is TransientLifetimeManager));
        }

        private static bool IsObjectDisposable(IBuilderContext context)
        {
            return typeof(IDisposable).IsAssignableFrom(context.BuildKey.Type);
        }

        private static bool IsObjectCreatedByContainer(IBuilderContext context)
        {
            return context.Existing == null;
        }

        private static ICurrentBuildNodePolicy GetCurrentBuildNodePolicy(IBuilderContext context)
        {
            ICurrentBuildNodePolicy policy = context.Policies.Get<ICurrentBuildNodePolicy>(context.BuildKey);

            if (policy == null)
            {
                policy = new CurrentBuildNodePolicy();

                context.Policies.Set<ICurrentBuildNodePolicy>(policy, context.BuildKey);
            }

            return policy;
        }

        private static bool WeAreResovlingContainerExtension(IBuilderContext context)
        {
            return typeof(UnityContainerExtension).IsAssignableFrom(context.BuildKey.Type);
        }
        
        private void CreateNewBuildTreeIfNecessary(BuildTreeItemNode parentNode)
        {
            if (WeNeedNewTree(parentNode))
            {
                this.CreateNewTree();
            }
        }

        private void AddToBuildTreeIfNecessary(BuildTreeItemNode newTreeNode)
        {
            if (this.CurrentBuildNode != null)
            {
                // This is a child node
                this.CurrentBuildNode.Children.Add(newTreeNode);
            }
        }

        private void AssertBuildTreeIsNotConstructedOutOfOrder(IBuilderContext context)
        {
            if (this.CurrentBuildNode.BuildKey != context.BuildKey)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.BuildTreeConstructedOutOfOrder,
                    this.CurrentBuildNode.BuildKey,
                    context.BuildKey);

                throw new InvalidOperationException(message);
            }
        }

        private void CreateNewTree()
        {
            using (new WriteLock(ReaderWriterLock))
            {
                this.BuildTrees.Add(this.CurrentBuildNode);
            }
        }

        private bool WeAreNotResponsibleForDisposal(ICurrentBuildNodePolicy policy)
        {
            return !string.Equals(this.Tag, policy.Tags.Peek(), StringComparison.Ordinal);
        }

        private void MarkThatWeTookPartInResolution(ICurrentBuildNodePolicy policy)
        {
            policy.Tags.Push(this.Tag);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (new UpgradeableReadLock(ReaderWriterLock))
                {
                    for (int index = this.BuildTrees.Count - 1; index >= 0; index--)
                    {
                        BuildTreeItemNode buildTree = this.BuildTrees[index];

                        this.DisposeItemsAndRemoveTree(buildTree);
                    }
                }
            }
        }

        private BuildTreeItemNode GetBuildTreeForInstance(object instance)
        {
            using (new ReadLock(ReaderWriterLock))
            {
                return this.BuildTrees.SingleOrDefault(x => x.Item != null && object.ReferenceEquals(x.Item, instance));
            }
        }

        private void PurgeDeadTrees()
        {
            // Need to enumerate in the reverse order because the trees that are torn down are removed from the set
            using (new UpgradeableReadLock(ReaderWriterLock))
            {
                for (int index = this.BuildTrees.Count - 1; index >= 0; index--)
                {
                    BuildTreeItemNode buildTree = this.BuildTrees[index];

                    if (buildTree.Item == null)
                    {
                        this.DisposeItemsAndRemoveTree(buildTree);
                    }
                }
            }
        }

        private void DisposeItemsAndRemoveTree(BuildTreeItemNode tree)
        {
            using (new WriteLock(ReaderWriterLock))
            {
                var visitor = new BuildTreeDisposer();

                tree.Accept(visitor);

                this.BuildTrees.Remove(tree);
            }
        }
    }
}