namespace Hydra.Unity.Tracking
{
    public interface ITreeItemVisitor
    {
        void Visit(DisposableItemNode node);

        void Visit(NonDisposableItemNode node);

        void Visit(SomeoneElsesProblem node);
    }
}