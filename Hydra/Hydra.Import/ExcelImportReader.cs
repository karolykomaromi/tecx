namespace Hydra.Import
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using DocumentFormat.OpenXml.Spreadsheet;

    public class ExcelImportReader<T> : IImportReader<T>
        where T : new()
    {
        private readonly Worksheet worksheet;
        private readonly SharedStringTable sharedStringTable;
        private readonly ValueWriterCollection writers;

        public ExcelImportReader(Worksheet worksheet, SharedStringTable sharedStringTable, ValueWriterCollection writers)
        {
            Contract.Requires(worksheet != null);
            Contract.Requires(sharedStringTable != null);
            Contract.Requires(writers != null);

            this.worksheet = worksheet;
            this.sharedStringTable = sharedStringTable;
            this.writers = writers;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Row rowWithPropertyNames = this.worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex == 1);

            if (rowWithPropertyNames == null)
            {
                yield break;
            }

            var properties = rowWithPropertyNames.Descendants<Cell>()
                .Select(cell => new
                {
                    PropertyName = ExcelHelper.GetCellValue(cell, this.sharedStringTable),
                    ColumnName = ExcelHelper.GetColumnName(cell)
                })
                .ToDictionary(x => x.ColumnName, x => x.PropertyName, StringComparer.OrdinalIgnoreCase);

            foreach (Row row in this.worksheet.Descendants<Row>().SkipWhile(r => r.RowIndex <= 1))
            {
                T item = new T();

                foreach (Cell cell in row.Descendants<Cell>())
                {
                    string columnName = ExcelHelper.GetColumnName(cell);

                    string propertyName;
                    if (properties.TryGetValue(columnName, out propertyName))
                    {
                        IValueWriter writer = this.writers[propertyName];

                        string value = ExcelHelper.GetCellValue(cell, this.sharedStringTable);

                        writer.Write(item, value, CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);
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