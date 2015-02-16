namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Hydra.Import.Data;
    using Hydra.Import.Results;
    using Hydra.Import.Settings;
    using Hydra.Import.ValueWriters;
    using NHibernate;

    public class ExcelBulkImport
    {
        private readonly IStatelessSession session;
        private readonly SpreadsheetDocument document;

        public ExcelBulkImport(IStatelessSession session, SpreadsheetDocument document)
        {
            Contract.Requires(session != null);
            Contract.Requires(document != null);

            this.session = session;
            this.document = document;
        }

        public ImportResult StartImport()
        {
            Worksheet metaSheet = this.document.GetMetaDataWorksheet();

            SharedStringTable sharedStringTable = this.document.GetSharedStringTable();

            ValueWriterCollection writers = ValueWriterCollectionBuilder.ForAllPropertiesOf<SheetToTypeMapping>();

            IExcelImportSettings metaImportSettings = new ExcelImportSettings();

            IDataReader<SheetToTypeMapping> metaReader = new ExcelDataReader<SheetToTypeMapping>(metaSheet, "Meta", sharedStringTable, writers, metaImportSettings);
            
            IExcelImportSettings importSettings = new ExcelImportSettings();

            var result = new CompositeImportResult();

            foreach (SheetToTypeMapping mapping in metaReader)
            {
                Type readerType = typeof(ExcelDataReader<>).MakeGenericType(mapping.Type);

                Worksheet worksheet = this.document.GetWorksheetByName(mapping.Sheet);

                if (worksheet == null)
                {
                    continue;
                }

                ValueWriterCollection w = ValueWriterCollectionBuilder.ForAllPropertiesOf(mapping.Type);

                object reader = Activator.CreateInstance(readerType, worksheet, mapping.Sheet, sharedStringTable, w, importSettings);

                Type writerType = typeof(NhBulkDataWriter<>).MakeGenericType(mapping.Type);

                object writer = Activator.CreateInstance(writerType, this.session);

                MethodInfo write = writerType.GetMethod("Write");

                ImportResult r = (ImportResult)write.Invoke(writer, new[] { reader });

                result.Add(r);
            }

            return result;
        }
    }
}