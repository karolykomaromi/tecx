namespace Hydra.Import.Test
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Xunit;

    public class ExcelDataWriterTests
    {
        [Fact]
        public void Should_Write_Items_To_Excel_Sheet()
        {
            using (Stream stream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();

                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                    SheetData sheetData = new SheetData();

                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    var sheet = new Sheet()
                        {
                            Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                            SheetId = 1,
                            Name = "Sheet 1"
                        };

                    sheets.AppendChild(sheet);
                }
            }
        }
    }

    public class ExcelDataWriter<T> : IDataWriter<T>
    {
        private readonly IExcelImportSettings settings;

        public ExcelDataWriter(IExcelImportSettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        public ImportResult Write(IEnumerable<T> items)
        {
            
        }
    }
}
