namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Hydra.Import.Messages;

    public abstract class ValueWriter : IValueWriter, IEquatable<IValueWriter>
    {
        private static readonly ConcurrentDictionary<string, IValueWriter> NullWriters = new ConcurrentDictionary<string, IValueWriter>();

        public abstract string PropertyName { get; }

        public static IValueWriter Null(string propertyName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            Contract.Ensures(Contract.Result<IValueWriter>() != null);

            IValueWriter writer = NullWriters.GetOrAdd(propertyName, pn => new NullValueWriter(pn));

            return writer;
        }

        public abstract ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture);

        public virtual bool Equals(IValueWriter other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.GetType() == other.GetType() &&
                string.Equals(this.PropertyName, other.PropertyName, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }
    }
}