namespace TecX.Unity.ContextualBinding
{
    public interface IRequestHistory
    {
        void Append(IRequest request);

        IRequest RemoveCurrent();

        IRequest Current();

        int Count { get; }
    }
}