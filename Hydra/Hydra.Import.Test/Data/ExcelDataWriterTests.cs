namespace Hydra.Import.Test.Data
{
    using System.IO;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using Hydra.Import.CellValueWriters;
    using Hydra.Import.Data;
    using Hydra.Import.Results;
    using Hydra.Import.Settings;
    using Hydra.Import.Test.ValueWriters;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class ExcelDataWriterTests
    {
        [Fact]
        public void Should_Write_Items_To_Excel_Sheet()
        {
#if DEBUG
            using (Stream stream = new FileStream(@"d:\tmp\ExcelDataWriterTests.xlsx", FileMode.Create))
#else
            using (Stream stream = new MemoryStream())
#endif
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    IExcelImportSettings settings = new ExcelImportSettings();

                    var writers = new ExcelCellValueWriterCollection(new DateTimeCellValueWriter(TypeHelper.GetProperty((ValueWriterTestObject x) => x.DateTime)));

                    var sut = new ExcelDataWriter<ValueWriterTestObject>(document, writers, settings);

                    var items = new[] { new ValueWriterTestObject { DateTime = TimeProvider.Now } };

                    ImportSucceeded result = Assert.IsType<ImportSucceeded>(sut.Write(items));
                }
            }
        }
    }
}
