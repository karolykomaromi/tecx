namespace Hydra.Import.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Hydra.Import.Results;

    [ContractClass(typeof(DataWriterContract<>))]
    public interface IDataWriter<in T>
    {
        ImportResult Write(IEnumerable<T> items);
    }

    [ContractClassFor(typeof(IDataWriter<>))]
    internal abstract class DataWriterContract<T> : IDataWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<ImportResult>() != null);

            return new ImportFailed();
        }
    }
}