// <auto-generated />
namespace TecX.Search.Data.EF.Reader
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;

    public static class EntityDataReaderExtensions
    {

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {

            // For anonymous type projections default to flattening related objects and not prefixing columns
            // The reason being that if the programmer has taken control of the projection, the default should
            // be to expose everying in the projection and not mess with the names.
            if (typeof(T).IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
            {
                var options = EntityDataReaderOptions.Default;
                options.FlattenRelatedObjects = true;
                options.PrefixRelatedObjectColumns = false;
                return new EntityDataReader<T>(collection, options);
            }
            return new EntityDataReader<T>(collection);
        }

        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, bool exposeNullableColumns, bool flattenRelatedObjects)
        {
            EntityDataReaderOptions options = new EntityDataReaderOptions(exposeNullableColumns, flattenRelatedObjects, true, false);

            return new EntityDataReader<T>(collection, options, null);
        }


        /// <summary>
        /// Enumerates the collection and copies the data into a DataTable.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">The collection to copy to a DataTable</param>
        /// <returns>A DataTable containing the scalar projection of the collection.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable t = new DataTable();
            t.Locale = System.Globalization.CultureInfo.CurrentCulture;
            t.TableName = typeof(T).Name;
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.ExposeNullableTypes = false;
            EntityDataReader<T> dr = new EntityDataReader<T>(collection, options);
            t.Load(dr);
            return t;
        }

        /// <summary>
        /// Wraps the collection in a DataReader, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">A collection to wrap in a DataReader</param>
        /// <param name="cx">The Entity Framework ObjectContext, used for metadata access</param>
        /// <returns>A DbDataReader wrapping the collection.</returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, ObjectContext context) where T : EntityObject
        {
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;
            return new EntityDataReader<T>(collection, options, context);
        }



        /// <summary>
        /// Wraps the collection in a DataReader, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collectin.</typeparam>
        /// <param name="collection">A collection to wrap in a DataReader</param>
        /// <param name="detachObjects">Option to detach each object in the collection from the ObjectContext.  This can reduce memory usage for queries returning large numbers of objects.</param>
        /// <param name="prefixRelatedObjectColumns">If True, qualify the related object keys, if False don't</param>
        /// <returns>A DbDataReader wrapping the collection.</returns>
        public static IDataReader AsDataReader<T>(this IEnumerable<T> collection, ObjectContext context, bool detachObjects, bool prefixRelatedObjectColumns) where T : EntityObject
        {
            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;
            options.PrefixRelatedObjectColumns = prefixRelatedObjectColumns;

            if (detachObjects)
            {
                return new EntityDataReader<T>(collection.DetachAllFrom(context), options, context);
            }
            return new EntityDataReader<T>(collection, options, context);
        }
        static IEnumerable<T> DetachAllFrom<T>(this IEnumerable<T> col, ObjectContext cx)
        {
            foreach (var t in col)
            {
                cx.Detach(t);
                yield return t;
            }
        }

        /// <summary>
        /// Enumerates the collection and copies the data into a DataTable, but also includes columns for the key attributes of related Entities.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">The collection to copy to a DataTable</param>
        /// <param name="cx">The Entity Framework ObjectContext, used for metadata access</param>
        /// <returns>A DataTable containing the scalar projection of the collection.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, ObjectContext context) where T : EntityObject
        {
            DataTable t = new DataTable();
            t.Locale = System.Globalization.CultureInfo.CurrentCulture;
            t.TableName = typeof(T).Name;

            EntityDataReaderOptions options = EntityDataReaderOptions.Default;
            options.RecreateForeignKeysForEntityFrameworkEntities = true;

            EntityDataReader<T> dr = new EntityDataReader<T>(collection, options, context);
            t.Load(dr);
            return t;
        }




    }
}