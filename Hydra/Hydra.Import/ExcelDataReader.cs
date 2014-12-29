namespace Hydra.Import
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using DocumentFormat.OpenXml.Spreadsheet;

    public class ExcelDataReader<T> : IDataReader<T>
        where T : new()
    {
        private readonly Worksheet worksheet;
        private readonly SharedStringTable sharedStringTable;
        private readonly ValueWriterCollection writers;
        private readonly IExcelImportSettings settings;
        private readonly ImportMessages messages;

        public ExcelDataReader(Worksheet worksheet, SharedStringTable sharedStringTable, ValueWriterCollection writers, IExcelImportSettings settings)
        {
            Contract.Requires(worksheet != null);
            Contract.Requires(sharedStringTable != null);
            Contract.Requires(writers != null);
            Contract.Requires(settings != null);

            this.worksheet = worksheet;
            this.sharedStringTable = sharedStringTable;
            this.writers = writers;
            this.settings = settings;
            this.messages = new ImportMessages();
        }

        public ImportMessages Messages
        {
            get { return this.messages; }
        }

        public IEnumerator<T> GetEnumerator()
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

                        this.Messages.Add(message);
                    }
                }

                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}