namespace Hydra.Import.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Import.Messages;
    using Hydra.Import.Settings;
    using Hydra.Import.ValueWriters;

    public class ExcelDataReader<T> : DataReader<T>
        where T : new()
    {
        private readonly Worksheet worksheet;
        private readonly string sheetName;
        private readonly SharedStringTable sharedStringTable;
        private readonly ValueWriterCollection writers;
        private readonly IExcelImportSettings settings;

        public ExcelDataReader(Worksheet worksheet, string sheetName, SharedStringTable sharedStringTable, ValueWriterCollection writers, IExcelImportSettings settings)
        {
            Contract.Requires(worksheet != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(sheetName));
            Contract.Requires(sharedStringTable != null);
            Contract.Requires(writers != null);
            Contract.Requires(settings != null);

            this.worksheet = worksheet;
            this.sheetName = sheetName;
            this.sharedStringTable = sharedStringTable;
            this.writers = writers;
            this.settings = settings;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            Row rowWithPropertyNames = this.worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex == this.settings.PropertyNamesRowIndex);

            if (rowWithPropertyNames == null)
            {
                yield break;
            }

            var properties = rowWithPropertyNames.Descendants<Cell>()
                .ToDictionary(
                    ExcelHelper.GetColumnName, 
                    c => ExcelHelper.GetCellValue(c, this.sharedStringTable), 
                    StringComparer.OrdinalIgnoreCase);

            foreach (Row row in this.worksheet.Descendants<Row>().SkipWhile(r => r.RowIndex <= this.settings.PropertyNamesRowIndex))
            {
                // skip empty rows
                if (row.Descendants<Cell>().All(c => string.IsNullOrWhiteSpace(c.InnerText)))
                {
                    continue;
                }

                T item = new T();

                foreach (Cell cell in row.Descendants<Cell>())
                {
                    string columnName = ExcelHelper.GetColumnName(cell);

                    string propertyName;
                    if (properties.TryGetValue(columnName, out propertyName))
                    {
                        IValueWriter writer = this.writers[propertyName];

                        string value = ExcelHelper.GetCellValue(cell, this.sharedStringTable);

                        ImportMessage message = writer.Write(item, value, this.settings.SourceCulture, this.settings.TargetCulture);

                        message.Location = new ExcelLocation(this.sheetName, cell.CellReference);

                        this.Messages.Add(message);
                    }
                }

                yield return item;
            }
        }
    }
}