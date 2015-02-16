namespace Hydra.Import.CellValueWriters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class ExcelCellValueWriterCollection
    {
        private readonly IDictionary<string, IExcelCellValueWriter> writers;

        public ExcelCellValueWriterCollection(params IExcelCellValueWriter[] writers)
        {
            this.writers = (writers ?? new IExcelCellValueWriter[0])
                .ToDictionary(
                    w => w.PropertyName,
                    w => w,
                    StringComparer.OrdinalIgnoreCase);
        }

        public IExcelCellValueWriter this[string propertyName]
        {
            get
            {
                Contract.Requires(propertyName != null);
                Contract.Ensures(Contract.Result<IExcelCellValueWriter>() != null);

                IExcelCellValueWriter writer;
                if (this.writers.TryGetValue(propertyName, out writer))
                {
                    return writer;
                }

                return ExcelCellValueWriter.Null;
            }
        }

        public void Add(IExcelCellValueWriter writer)
        {
            Contract.Requires(writer != null);

            this.writers[writer.PropertyName] = writer;
        }
    }
}