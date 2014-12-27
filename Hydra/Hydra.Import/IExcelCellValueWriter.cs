namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DocumentFormat.OpenXml.Spreadsheet;

    [ContractClass(typeof(ExcelCellValueWriterContract))]
    public interface IExcelCellValueWriter
    {
        ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture);
    }

    [ContractClassFor(typeof(IExcelCellValueWriter))]
    internal abstract class ExcelCellValueWriterContract : IExcelCellValueWriter
    {
        public ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            Contract.Requires(target != null);
            Contract.Requires(sourceCulture != null);
            Contract.Requires(targetCulture != null);
            Contract.Ensures(Contract.Result<ImportMessage>() != null);

            return ImportMessage.Empty;

        }
    }
}