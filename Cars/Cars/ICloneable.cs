namespace Cars
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(CloneableContract<>))]
    public interface ICloneable<out T>
        where T : class
    {
        T Clone();
    }

    [ContractClassFor(typeof(ICloneable<>))]
    internal abstract class CloneableContract<T> : ICloneable<T>
        where T : class
    {
        public T Clone()
        {
            Contract.Ensures(Contract.Result<T>() != null);

            return Default.Value<T>();
        }
    }
}