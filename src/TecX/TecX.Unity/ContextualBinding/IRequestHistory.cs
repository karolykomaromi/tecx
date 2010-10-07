namespace TecX.Unity.ContextualBinding
{
    public interface IRequestHistory
    {
        void Push(IRequest request);

        IRequest Pop();

        IRequest Peek();

        int Count { get; }
    }
}