namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Hydra.Infrastructure;

    public class ValueWriterCollectionBuilder : Builder<ValueWriterCollection>
    {
        private readonly List<Func<IValueWriter>> writerFactories;

        public ValueWriterCollectionBuilder()
        {
            this.writerFactories = new List<Func<IValueWriter>>();
        }

        public static Func<IValueWriter> GetWriterFactory(PropertyInfo property)
        {
            Contract.Requires(property != null);
            Contract.Ensures(Contract.Result<Func<IValueWriter>>() != null);

            if (property.PropertyType == typeof(string))
            {
                return () => new StringValueWriter(property);
            }

            if (property.PropertyType == typeof(DateTime))
            {
                return () => new DateTimeValueWriter(property);
            }

            if (property.PropertyType == typeof(long))
            {
                return () => new Int64ValueWriter(property);
            }

            if (property.PropertyType == typeof(int))
            {
                return () => new Int32ValueWriter(property);
            }

            if (property.PropertyType == typeof(decimal))
            {
                return () => new DecimalValueWriter(property);
            }

            if (property.PropertyType == typeof(double))
            {
                return () => new DoubleValueWriter(property);
            }

            if (property.PropertyType == typeof(float))
            {
                return () => new FloatValueWriter(property);
            }

            if (property.PropertyType == typeof(Type))
            {
                return () => new TypeValueWriter(property);
            }

            if (property.PropertyType == typeof(CultureInfo))
            {
                return () => new CultureInfoValueWriter(property);
            }

            return () => ValueWriter.Null(property.Name);
        }

        public static ValueWriterCollectionBuilder ForAllPropertiesOf<T>()
        {
            return ValueWriterCollectionBuilder.ForAllPropertiesOf(typeof(T));
        }

        public static ValueWriterCollectionBuilder ForAllPropertiesOf(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<ValueWriterCollectionBuilder>() != null);

            var builder = new ValueWriterCollectionBuilder();

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                Func<IValueWriter> writerFactory = ValueWriterCollectionBuilder.GetWriterFactory(property);

                builder.writerFactories.Add(writerFactory);
            }

            return builder;
        }

        public override ValueWriterCollection Build()
        {
            return new ValueWriterCollection(this.writerFactories.Select(f => f()).ToArray());
        }
    }
}