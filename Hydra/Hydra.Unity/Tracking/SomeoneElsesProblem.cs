namespace Hydra.Unity.Tracking
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    public class SomeoneElsesProblem : NonDisposableItemNode
    {
        public SomeoneElsesProblem(NamedTypeBuildKey buildKey, BuildTreeItemNode parentNode = null)
            : base(buildKey, parentNode)
        {
        }

        public override void Accept(ITreeItemVisitor visitor)
        {
            Contract.Requires(visitor != null);

            visitor.Visit(this);
        }
    }
}