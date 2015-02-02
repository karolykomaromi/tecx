namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DocumentFormat.OpenXml.Spreadsheet;

    [ContractClass(typeof(ExcelCellValueWriterContract))]
    public interface IExcelCellValueWriter
    {
        string PropertyName { get; }

        ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture);
    }

    [ContractClassFor(typeof(IExcelCellValueWriter))]
    internal abstract class ExcelCellValueWriterContract : IExcelCellValueWriter
    {
        public string PropertyName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return string.Empty;
            }
        }

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