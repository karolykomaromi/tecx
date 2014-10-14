namespace Hydra.Import.Test
{
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Xunit;

    public class ExcelImportTest
    {
        [Fact]
        public void Should_Read_Xlsx()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    SharedStringTable sharedStringTable = document.GetSharedStringTable();

                    Worksheet sheet = document.GetMetaDataWorksheet();

                    Assert.NotNull(sharedStringTable);
                    Assert.NotNull(sheet);
                }
            }
        }
    }
}
