namespace TecX.Data
{
    public class ObjectDataReaderOptions
    {
        public ObjectDataReaderOptions(
            bool exposeNullableTypes,
            bool flattenRelatedObjects,
            bool prefixRelatedObjectColumns)
        {
            this.ExposeNullableTypes = exposeNullableTypes;
            this.FlattenRelatedObjects = flattenRelatedObjects;
            this.PrefixRelatedObjectColumns = prefixRelatedObjectColumns;
        }

        public static ObjectDataReaderOptions Default
        {
            get { return new ObjectDataReaderOptions(true, false, true); }
        }

        /// <summary>
        /// If true nullable value types are returned directly by the DataReader.
        /// If false, the DataReader will expose non-nullable value types and return DbNull.Value
        /// for null values.  
        /// When loading a DataTable this option must be set to True, since DataTable does not support
        /// nullable types.
        /// </summary>
        public bool ExposeNullableTypes { get; set; }

        /// <summary>
        /// If True then the DataReader will project scalar properties from related objects in addition
        /// to scalar properties from the main object.  This is especially useful for custom projecttions like
        ///         var q = from od in db.SalesOrderDetail
        ///         select new
        ///         {
        ///           od,
        ///           ProductID=od.Product.ProductID,
        ///           ProductName=od.Product.Name
        ///         };
        /// Related objects assignable to EntityKey, EntityRelation, and IEnumerable are excluded.
        /// 
        /// If False, then only scalar properties from teh main object will be projected.         
        /// </summary>
        public bool FlattenRelatedObjects { get; set; }

        /// <summary>
        /// If True columns projected from related objects will have column names prefixed by the
        /// name of the relating property.  This appies to either from setting FlattenRelatedObjects to True,
        /// or RecreateForeignKeysForEntityFrameworkEntities to True.
        /// 
        /// If False columns will be created for related properties that are not prefixed.  This can lead
        /// to column name collision.
        /// </summary>
        public bool PrefixRelatedObjectColumns { get; set; }
    }
}