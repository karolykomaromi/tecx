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
            Contract.Requires(instance != null);
            Contract.Requires(!(instance is IDisposable));

            if (this.item != null)
            {
                throw new InvalidOperationException("Instance already assigned");
            }

            this.item = new WeakReference(instance);
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            Contract.Requires(visitor != null);
            
            visitor.Visit(this);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}