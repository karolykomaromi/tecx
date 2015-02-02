namespace Hydra.Import
{
    using System.Globalization;
    using DocumentFormat.OpenXml.Spreadsheet;

    public abstract class ExcelCellValueWriter : IExcelCellValueWriter
    {
        public static readonly IExcelCellValueWriter Null = new NullCellValueWriter();

        public abstract string PropertyName { get; }

        public abstract ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture);

        private class NullCellValueWriter : IExcelCellValueWriter
        {
            public string PropertyName
            {
                get { return string.Empty; }
            }

            public ImportMessage Write(Cell target, object value, CultureInfo sourceCulture, CultureInfo targetCulture)
            {
                return ImportMessage.Empty;
            }
        }
    }
}