namespace Infrastructure.Data
{
    public class ObjectDataReaderOptions
    {
        public ObjectDataReaderOptions(bool exposeNullableTypes, bool flattenRelatedObjects, bool prefixRelatedObjectColumns)
        {
            this.ExposeNullableTypes = exposeNullableTypes;
            this.FlattenRelatedObjects = flattenRelatedObjects;
            this.PrefixRelatedObjectColumns = prefixRelatedObjectColumns;
        }

        public static ObjectDataReaderOptions Default
        {
            get { return new ObjectDataReaderOptions(true, false, true); }
        }

        public bool ExposeNullableTypes { get; set; }

        public bool FlattenRelatedObjects { get; set; }

        public bool PrefixRelatedObjectColumns { get; set; }
    }
}