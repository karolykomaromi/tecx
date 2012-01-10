namespace TecX.Search.Data.Test.TestObjects
{
    using System;
    using System.Data.SqlClient;
    using System.Reflection;

    using TecX.Common;

    public class SqlBulkCopyColumnMappingsBuilder
    {
        private readonly SqlBulkCopyColumnMappingCollection mappings;

        public SqlBulkCopyColumnMappingsBuilder(SqlBulkCopyColumnMappingCollection mappings)
        {
            Guard.AssertNotNull(mappings, "mappings");

            this.mappings = mappings;
        }

        public SqlBulkCopyColumnMappingsBuilder MapAllPropertiesOf(Type typeToMap)
        {
            Guard.AssertNotNull(typeToMap, "typeToMap");

            var properties = typeToMap.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                this.mappings.Add(property.Name, property.Name);
            }

            return this;
        }

        public SqlBulkCopyColumnMappingsBuilder IgnoreAll(Predicate<SqlBulkCopyColumnMapping> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            for (int i = this.mappings.Count - 1; i > 0; i--)
            {
                if (predicate(this.mappings[i]))
                {
                    this.mappings.RemoveAt(i);
                }
            }

            return this;
        }
    }
}