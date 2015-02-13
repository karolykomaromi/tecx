namespace Hydra.Infrastructure.Reflection
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(DuckTypeGeneratorContract))]
    public interface IDuckTypeGenerator
    {
        T Duck<T>(object duck) where T : class;
    }

    [ContractClassFor(typeof(IDuckTypeGenerator))]
    internal abstract class DuckTypeGeneratorContract : IDuckTypeGenerator
    {
        public T Duck<T>(object duck) where T : class
        {
            Contract.Requires(duck != null);
            Contract.Ensures(Contract.Result<T>() != null);

            return default(T);
        }
    }
}