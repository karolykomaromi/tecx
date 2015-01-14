namespace Hydra.Import.Test
{
    using System.IO;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class ExcelDataWriterTests
    {
        [Fact]
        public void Should_Write_Items_To_Excel_Sheet()
        {
            ////using (Stream stream = new MemoryStream())
            using (Stream stream = new FileStream(@"d:\tmp\ExcelDataWriterTests.xlsx", FileMode.Create))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    IExcelImportSettings settings = new ExcelImportSettings();

                    var writers = new ExcelCellValueWriterCollection(new DateTimeCellValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.DateTime)));

                    var sut = new ExcelDataWriter<ValueWriterTestObject>(document, writers, settings);

                    var items = new[] { new ValueWriterTestObject { DateTime = TimeProvider.Now } };

                    ImportResult result = sut.Write(items);
                }
            }
        }
    }
}
