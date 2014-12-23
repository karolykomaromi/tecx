namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
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

            IImportReader<SheetToTypeMapping> metaReader = new ExcelImportReader<SheetToTypeMapping>(metaSheet, sharedStringTable, writers, metaImportSettings);
            
            IExcelImportSettings importSettings = new ExcelImportSettings();

            var result = new CompositeImportResult();

            foreach (SheetToTypeMapping mapping in metaReader)
            {
                Type readerType = typeof(ExcelImportReader<>).MakeGenericType(mapping.Type);

                Worksheet worksheet = this.document.GetWorksheetByName(mapping.Sheet);

                if (worksheet == null)
                {
                    continue;
                }

                ValueWriterCollection w = ValueWriterCollectionBuilder.ForAllPropertiesOf(mapping.Type);

                object reader = Activator.CreateInstance(readerType, worksheet, sharedStringTable, w, importSettings);

                Type writerType = typeof(NhBulkImportWriter<>).MakeGenericType(mapping.Type);

                object writer = Activator.CreateInstance(writerType, this.session);

                MethodInfo write = writerType.GetMethod("Write");

                ImportResult r = (ImportResult)write.Invoke(writer, new[] { reader });

                result.Add(r);
            }

            return result;
        }
    }
}