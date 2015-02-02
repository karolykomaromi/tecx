namespace TecX.Search.Data.Test.TestObjects
{
    using System;
    using System.Data.SqlClient;

    using TecX.Common;

    public static class SqlBulkCopyColumnMappingCollectionExtensions
    {
        public static SqlBulkCopyColumnMappingsBuilder MapAllPropertiesOf(this SqlBulkCopyColumnMappingCollection mappings, Type typeToMap)
        {
            Guard.AssertNotNull(mappings, "mappings");
            Guard.AssertNotNull(typeToMap, "typeToMap");

            var builder = new SqlBulkCopyColumnMappingsBuilder(mappings);

            return builder.MapAllPropertiesOf(typeToMap);
        }
    }
}