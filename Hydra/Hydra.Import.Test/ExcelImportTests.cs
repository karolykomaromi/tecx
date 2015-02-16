namespace Hydra.Import.Test
{
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using Hydra.Import.Results;
    using Moq;
    using NHibernate;
    using Xunit;

    public class ExcelImportTests
    {
        [Fact]
        public void Should_Import_Data_From_All_Sheets()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Import001))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    var session = new Mock<IStatelessSession>();

                    var import = new ExcelBulkImport(session.Object, document);

                    ImportResult result = import.StartImport();

                    Assert.Equal(0, result.Messages.Count);
                }
            }
        }
    }
}
