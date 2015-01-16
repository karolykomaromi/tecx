namespace Hydra.Import.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class ExcelDataReaderTests
    {
        [Fact]
        public void Should_Read_ResourceItems_From_Excel_Sheet()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.GetSharedStringTable();

                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .First(s => string.Equals("Resources", s.Name, StringComparison.OrdinalIgnoreCase));

                    WorksheetPart part = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                    Worksheet worksheet = part.Worksheet;

                    ValueWriterCollection writers = ValueWriterCollectionBuilder.ForAllPropertiesOf<ResourceItem>();

                    IExcelImportSettings settings = new ExcelImportSettings();

                    IDataReader<ResourceItem> items = new ExcelDataReader<ResourceItem>(worksheet, sharedStringTable, writers, settings);

                    ResourceItem[] actual = items.ToArray();

                    Assert.Equal("FOO", actual[0].Name);
                    Assert.Equal(Cultures.GermanNeutral, actual[0].Language);
                    Assert.Equal("FOO_DE", actual[0].Value);

                    Assert.Equal("BAR.BAZ", actual[3].Name);
                    Assert.Equal(Cultures.EnglishNeutral, actual[3].Language);
                    Assert.Equal("BAR.BAZ_EN", actual[3].Value);
                }
            }
        }
    }
}
