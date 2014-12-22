namespace Hydra.Import.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
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
                    Worksheet meta = document.GetMetaDataWorksheet();

                    SharedStringTable sharedStringTable = document.GetSharedStringTable();

                    ValueWriterCollection writers = new ValueWriterCollectionBuilder<SheetToTypeMapping>().ForAll();

                    IExcelImportSettings settings = new ExcelImportSettings();

                    IImportReader<SheetToTypeMapping> metaReader = new ExcelImportReader<SheetToTypeMapping>(meta, sharedStringTable, writers, settings);

                    foreach (SheetToTypeMapping mapping in metaReader)
                    {
                        
                    }
                }
            }
        }
    }

    [DebuggerDisplay("Sheet={Sheet} Type={Type.AssemblyQualifiedName}")]
    public class SheetToTypeMapping
    {
        public string Sheet { get; set; }

        public Type Type { get; set; }
    }
}
