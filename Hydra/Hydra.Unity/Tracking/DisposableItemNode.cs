namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    [DebuggerDisplay("Disposable Type={BuildKey.Type} Name={BuildKey.Name} Count={Children.Count}")]
    public class DisposableItemNode : BuildTreeItemNode
    {
        private object item;

        public DisposableItemNode(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
            : base(buildKey, parentNode)
        {
        }

        public override object Item
        {
            get { return this.item; }
        }

        public override void AssignInstance(object instance)
        {
            Contract.Requires(instance != null);
            Contract.Requires(instance is IDisposable);

            if (this.item != null)
            {
                throw new InvalidOperationException("Instance already assigned");
            }

            this.item = instance;
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            Contract.Requires(visitor != null);

            visitor.Visit(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable disposable = this.item as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this.item = null;
            }
        }
    }
}