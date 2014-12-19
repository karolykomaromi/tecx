namespace Hydra.Import.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Nh.Infrastructure.I18n;
    using Xunit;

    public class ExcelImportReaderTests
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

                    ValueWriterCollection writers = new ValueWriterCollectionBuilder<ResourceItem>().ForAll();

                    IExcelImportSettings settings = new ExcelImportSettings();

                    IImportReader<ResourceItem> items = new ExcelImportReader<ResourceItem>(worksheet, sharedStringTable, writers, settings);

                    ResourceItem[] actual = items.ToArray();

                    Assert.Equal("FOO", actual[0].Name);
                    Assert.Equal("DE", actual[0].TwoLetterISOLanguageName);
                    Assert.Equal("FOO_DE", actual[0].Value);

                    Assert.Equal("BAR.BAZ", actual[3].Name);
                    Assert.Equal("EN", actual[3].TwoLetterISOLanguageName);
                    Assert.Equal("BAR.BAZ_EN", actual[3].Value);
                }
            }
        }
    }
}
