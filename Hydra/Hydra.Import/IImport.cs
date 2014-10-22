namespace Hydra.Import
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ImportContract))]
    public interface IImport
    {
        ImportResult Start();
    }

    [ContractClassFor(typeof(IImport))]
    internal abstract class ImportContract : IImport
    {
        public ImportResult Start()
        {
            Contract.Ensures(Contract.Result<ImportResult>() != null);

            return new ImportResult();
        }
    }
}