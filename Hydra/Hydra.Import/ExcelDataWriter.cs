namespace Hydra.Import
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using DocumentFormat.OpenXml.Validation;

    public class ExcelDataWriter<T> : IDataWriter<T>
    {
        private readonly IExcelImportSettings settings;

        private readonly SpreadsheetDocument document;

        private readonly ExcelCellValueWriterCollection writers;

        public ExcelDataWriter(SpreadsheetDocument document, ExcelCellValueWriterCollection writers, IExcelImportSettings settings)
        {
            Contract.Requires(settings != null);
            Contract.Requires(document != null);
            Contract.Requires(writers != null);

            this.settings = settings;
            this.document = document;
            this.writers = writers;
        }

        public ImportResult Write(IEnumerable<T> items)
        {
            List<ImportMessage> messages = new List<ImportMessage>();

            WorkbookPart workbookPart = this.document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            workbookPart.Workbook.Save();

            SharedStringTablePart shardSharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>();
            shardSharedStringTablePart.SharedStringTable = new SharedStringTable();
            shardSharedStringTablePart.SharedStringTable.Save();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            SheetData sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);
            Sheets sheets = this.document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            var sheet = new Sheet()
                {
                    Id = this.document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = typeof(T).Name
                };

            sheets.AppendChild(sheet);

            WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

            workbookStylesPart.Stylesheet = new StyleSheetBuilder();

            workbookStylesPart.Stylesheet.Save();

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead).ToArray();

            Row headers = new Row { RowIndex = 1 };

            sheetData.AppendChild(headers);

            for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
            {
                string columnName = ExcelHelper.GetColumnName(columnIndex + 1);

                Cell header = new Cell
                {
                    CellReference = columnName + 1,
                    DataType = CellValues.InlineString
                };

                InlineString inlineString = new InlineString();

                Text text = new Text { Text = properties[columnIndex].Name };

                inlineString.AppendChild(text);

                header.AppendChild(inlineString);

                headers.AppendChild(header);
            }

            uint rowIndex = 2;

            foreach (T item in items)
            {
                Row row = new Row { RowIndex = rowIndex };

                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    PropertyInfo property = properties[columnIndex];

                    string columnName = ExcelHelper.GetColumnName(columnIndex + 1);

                    string cellReference = columnName + rowIndex;

                    Cell cell = new Cell { CellReference = cellReference };

                    this.writers[property.Name].Write(cell, property.GetValue(item), this.settings.SourceCulture, this.settings.TargetCulture);

                    row.AppendChild(cell);
                }

                sheetData.AppendChild(row);

                rowIndex++;
            }

            workbookPart.Workbook.Save();

            OpenXmlValidator validator = new OpenXmlValidator();

            messages.AddRange(validator.Validate(this.document).Select(error => new ExcelValidationError(error.Description, error)));

            if (messages.Count > 0)
            {
                var result = new ImportFailed();

                result.Messages.Add(messages);
            }

            return new ImportSucceeded();
        }
    }
}