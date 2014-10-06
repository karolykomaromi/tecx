namespace Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    public static class ObjectDataReaderExtensions
    {
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {
            // For anonymous type projections default to flattening related objects and not prefixing columns
            // The reason being that if the programmer has taken control of the projection, the default should
            // be to expose everying in the projection and not mess with the names.
            if (typeof(T).IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
            {
                var options = ObjectDataReaderOptions.Default;
                options.FlattenRelatedObjects = true;
                options.PrefixRelatedObjectColumns = false;
                return new ObjectDataReader<T>(collection, options);
            }

            return new ObjectDataReader<T>(collection);
        }

        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, bool exposeNullableColumns, bool flattenRelatedObjects)
        {
            ObjectDataReaderOptions options = new ObjectDataReaderOptions(exposeNullableColumns, flattenRelatedObjects, true);

            return new ObjectDataReader<T>(collection, options);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable t = new DataTable
                {
                    Locale = CultureInfo.CurrentCulture,
                    TableName = typeof(T).Name
                };

            ObjectDataReaderOptions options = ObjectDataReaderOptions.Default;
            options.ExposeNullableTypes = false;
            ObjectDataReader<T> dr = new ObjectDataReader<T>(collection, options);
            t.Load(dr);
            return t;
        }
    }
}