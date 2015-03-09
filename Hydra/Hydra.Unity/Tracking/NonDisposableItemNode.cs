namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    [DebuggerDisplay("NonDisposable Type={BuildKey.Type} Name={BuildKey.Name} Count={Children.Count}")]
    public class NonDisposableItemNode : BuildTreeItemNode
    {
        private WeakReference item;

        public NonDisposableItemNode(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
            : base(buildKey, parentNode)
        {
            Contract.Requires(buildKey != null);
        }

        public override object Item
        {
            get
            {
                if (this.item != null && this.item.IsAlive)
                {
                    return this.item.Target;
                }

                return null;
            }
        }

        public override void AssignInstance(object instance)
        {
            if (this.item != null)
            {
                throw new InvalidOperationException(Properties.Resources.InstanceAlreadyAssigned);
            }

            this.item = new WeakReference(instance);
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}