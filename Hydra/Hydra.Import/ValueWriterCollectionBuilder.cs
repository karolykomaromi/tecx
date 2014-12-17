namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure;

    public class ValueWriterCollectionBuilder<T> : Builder<ValueWriterCollection>
    {
        private readonly List<Func<IValueWriter>> writerFactories;
        
        public ValueWriterCollectionBuilder()
        {
            this.writerFactories = new List<Func<IValueWriter>>();
        }

        public static Func<IValueWriter> GetWriterFactory(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return () => new StringValueWriter(property);
            }

            return () => ValueWriter.Null;
        }

        public ValueWriterCollectionBuilder<T> For<TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            MemberExpression expression = (MemberExpression)propertySelector.Body;

            PropertyInfo property = (PropertyInfo)expression.Member;

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

                Func<IValueWriter> writerFactory = GetWriterFactory(property);

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