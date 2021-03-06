namespace Hydra.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    [ContractClass(typeof(BuildTreeItemNodeContract))]
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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Second class contains contracts.")]
    [ContractClassFor(typeof(BuildTreeItemNode))]
    internal abstract class BuildTreeItemNodeContract : BuildTreeItemNode
    {
        protected BuildTreeItemNodeContract(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
            : base(buildKey, parentNode)
        {
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            Contract.Requires(visitor != null);
        }
    }
}