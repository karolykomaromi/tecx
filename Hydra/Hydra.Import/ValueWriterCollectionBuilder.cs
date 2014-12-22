namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Reflection;

    public class ValueWriterCollectionBuilder<T> : Builder<ValueWriterCollection>
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

            return () => ValueWriter.Null;
        }

        public ValueWriterCollectionBuilder<T> For<TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            PropertyInfo property = TypeHelper.GetProperty(propertySelector);

            Func<IValueWriter> writerFactory = GetWriterFactory(property);

            this.writerFactories.Add(writerFactory);

            return this;
        }

        public ValueWriterCollectionBuilder<T> ForAll()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                PropertyInfo p = property;

                Func<IValueWriter> writerFactory = GetWriterFactory(p);

                this.writerFactories.Add(writerFactory);
            }

            return this;
        }

        public override ValueWriterCollection Build()
        {
            return new ValueWriterCollection(this.writerFactories.Select(f => f()).ToArray());
        }
    }
}