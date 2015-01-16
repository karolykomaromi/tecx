namespace Hydra.Import
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using DocumentFormat.OpenXml;
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

            Stylesheet stylesheet = new Stylesheet();

            workbookStylesPart.Stylesheet = stylesheet;

            AddBasicStyles(stylesheet);

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

                foreach (ImportMessage message in messages)
                {
                    result.Messages.Add(message);
                }
            }

            return new ImportSucceeded();
        }

        private static void AddBasicStyles(Stylesheet stylesheet)
        {
            // Numbering formats (x:numFmts)
            stylesheet.InsertAt<NumberingFormats>(new NumberingFormats(), 0);

            // Currency
            stylesheet.GetFirstChild<NumberingFormats>().InsertAt<NumberingFormat>(
                new NumberingFormat
                    {
                        NumberFormatId = 164,
                        FormatCode = "#.##0,00"
                        + "\\ \"" + CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol + "\""
                    },
                0);

            // Fonts (x:fonts)
            stylesheet.InsertAt<Fonts>(new Fonts(), 1);
            stylesheet.GetFirstChild<Fonts>().InsertAt<Font>(
                new Font
                   {
                       FontSize = new FontSize { Val = 11 },
                       FontName = new FontName { Val = "Calibri" }
                   },
                0);

            // Fills (x:fills)
            stylesheet.InsertAt<Fills>(new Fills(), 2);
            stylesheet.GetFirstChild<Fills>().InsertAt<Fill>(
                new Fill
                    {
                        PatternFill = new PatternFill
                            {
                                PatternType = new EnumValue<PatternValues> { Value = PatternValues.None }
                            }
                    },
                0);

            // Borders (x:borders)
            stylesheet.InsertAt<Borders>(new Borders(), 3);
            stylesheet.GetFirstChild<Borders>().InsertAt<Border>(
                new Border
                    {
                        LeftBorder = new LeftBorder(),
                        RightBorder = new RightBorder(),
                        TopBorder = new TopBorder(),
                        BottomBorder = new BottomBorder(),
                        DiagonalBorder = new DiagonalBorder()
                    },
                0);

            // Cell style formats (x:CellStyleXfs)
            stylesheet.InsertAt<CellStyleFormats>(new CellStyleFormats(), 4);

            stylesheet.GetFirstChild<CellStyleFormats>().InsertAt<CellFormat>(
                new CellFormat
                    {
                        NumberFormatId = 0,
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0
                    },
                0);

            // Cell formats (x:CellXfs)
            stylesheet.InsertAt<CellFormats>(new CellFormats(), 5);

            // General text
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                    {
                        FormatId = 0,
                        NumberFormatId = 0
                    },
                CellFormatIndices.Text);

            // Date
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                    {
                        ApplyNumberFormat = true,
                        FormatId = 0,
                        NumberFormatId = 22,
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0
                    },
                CellFormatIndices.Date);

            // Currency
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat
                    {
                        ApplyNumberFormat = true,
                        FormatId = 0,
                        NumberFormatId = 164,
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0
                    },
                CellFormatIndices.Currency);

            // Percentage
            stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
                new CellFormat()
                   {
                       ApplyNumberFormat = true,
                       FormatId = 0,
                       NumberFormatId = 10,
                       FontId = 0,
                       FillId = 0,
                       BorderId = 0
                   },
                CellFormatIndices.Percentage);
        }
    }
}