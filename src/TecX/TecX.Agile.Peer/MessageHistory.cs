namespace TecX.Agile.Peer
{
    public abstract class MessageHistory<T>
    {
        public abstract int Count { get; }

        public abstract void Add(T item);

        public abstract bool Contains(T candidate);

        public abstract bool Remove(T item);
    }
}