namespace Hydra.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    public abstract class BuildTreeItemNode : IDisposable
    {
        private readonly List<BuildTreeItemNode> children;
        private readonly NamedTypeBuildKey buildKey;
        private readonly BuildTreeItemNode parent;

        protected BuildTreeItemNode(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
        {
            Contract.Requires(buildKey != null);

            this.buildKey = buildKey;
            this.parent = parentNode;
            this.children = new List<BuildTreeItemNode>();
        }

        public NamedTypeBuildKey BuildKey
        {
            get { return this.buildKey; }
        }

        public IList<BuildTreeItemNode> Children
        {
            get { return this.children; }
        }

        public BuildTreeItemNode Parent
        {
            get { return this.parent; }
        }

        public abstract object Item { get; }

        public abstract void AssignInstance(object instance);

        public abstract void Accept(ITreeItemVisitor visitor);

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}