namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(FreezableContract<>))]
    public interface IFreezable<out T>
        where T : class
    {
        bool IsMutable { get; }

        T Freeze();
    }

    [ContractClassFor(typeof(IFreezable<>))]
    internal abstract class FreezableContract<T> : IFreezable<T>
        where T : class
    {
        public abstract bool IsMutable { get; }

        public T Freeze()
        {
            Contract.Ensures(Contract.Result<T>() != null);

            return Default.Value<T>();
        }
    }
}