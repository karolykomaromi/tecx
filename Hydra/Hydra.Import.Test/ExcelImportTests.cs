namespace Hydra.Import.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
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
                    Worksheet meta = document.GetMetaDataWorksheet();

                    SharedStringTable sharedStringTable = document.GetSharedStringTable();

                    ValueWriterCollection writers = new ValueWriterCollectionBuilder<SheetToTypeMapping>().ForAll();

                    IExcelImportSettings metaImportSettings = new ExcelImportSettings();

                    IImportReader<SheetToTypeMapping> metaReader = new ExcelImportReader<SheetToTypeMapping>(meta, sharedStringTable, writers, metaImportSettings);

                    IExcelImportSettings settings = new ExcelImportSettings();

                    IStatelessSession session = null;

                    foreach (SheetToTypeMapping mapping in metaReader)
                    {
                        Type readerType = typeof(ExcelImportReader<>).MakeGenericType(mapping.Type);

                        Worksheet worksheet = document.GetWorksheetByName(mapping.Sheet);

                        if (worksheet == null)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        //public static IImportWriter<T> GetReaderWriterPipeline<T>(Worksheet sheet, SharedStringTable sharedStringTable, IExcelImportSettings settings)
        //{
        //    ValueWriterCollection writers = new ValueWriterCollectionBuilder<T>().ForAll();

        //    IImportReader<T> reader = new ExcelImportReader<T>(sheet, sharedStringTable, writers, settings);

        //    return reader;
        //}
    }

    [DebuggerDisplay("Sheet={Sheet} Type={Type.AssemblyQualifiedName}")]
    public class SheetToTypeMapping
    {
        public string Sheet { get; set; }

        public Type Type { get; set; }
    }
}
