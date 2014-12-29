namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DocumentFormat.OpenXml.Drawing.Wordprocessing;
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

    public class DateTimeCellValueWriter : IExcelCellValueWriter
    {
        public ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (value == null)
            {
                return ImportMessage.Empty;
            }

            if (!(value is DateTime))
            {
                string message = string.Format(
                    Properties.Resources.ErrorValueIsOfWrongType, 
                    typeof(DateTime).AssemblyQualifiedName, 
                    value.GetType().AssemblyQualifiedName);

                return new Error(message);
            }

            DateTime dt = (DateTime)value;

            target.DataType = CellValues.Date;
            InlineString inlineString = new InlineString();
            Text text = new Text{ Text = dt.ToOADate().ToString("R", targetCulture) };

            inlineString.AppendChild(text);
            target.AppendChild(inlineString);

            return ImportMessage.Empty;
        }
    }
}