namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class ValueWriterCollection
    {
        private readonly IDictionary<string, IValueWriter> writers;

        public ValueWriterCollection(params IValueWriter[] writers)
        {
            this.writers = (writers ?? new IValueWriter[0])
                .ToDictionary(
                    w => w.PropertyName,
                    w => w,
                    StringComparer.OrdinalIgnoreCase);
        }

        public IValueWriter this[string propertyName]
        {
            get
            {
                Contract.Requires(propertyName != null);
                Contract.Ensures(Contract.Result<IValueWriter>() != null);

                IValueWriter writer;
                if (this.writers.TryGetValue(propertyName, out writer))
                {
                    return writer;
                }

                return ValueWriter.Null(propertyName);
            }
        }

        public void Add(IValueWriter writer)
        {
            Contract.Requires(writer != null);

            this.writers[writer.PropertyName] = writer;
        }
    }
}