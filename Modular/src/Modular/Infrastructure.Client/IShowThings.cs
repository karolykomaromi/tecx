namespace Infrastructure
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ShowThingsContract<>))]
    public interface IShowThings<in T>
        where T : class
    {
        void Show(T thing);
    }

    [ContractClassFor(typeof(IShowThings<>))]
    public abstract class ShowThingsContract<T> : IShowThings<T>
        where T : class
    {
        public void Show(T thing)
        {
            Contract.Requires(thing != null);
        }
    }
}