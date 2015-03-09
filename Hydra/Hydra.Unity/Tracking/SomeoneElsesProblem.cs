namespace Hydra.Unity.Tracking
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    [DebuggerDisplay("SomeoneElsesProblem Type={BuildKey.Type} Name={BuildKey.Name} Count={Children.Count}")]
    public class SomeoneElsesProblem : NonDisposableItemNode
    {
        public SomeoneElsesProblem(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
            : base(buildKey, parentNode)
        {
            Contract.Requires(buildKey != null);
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}