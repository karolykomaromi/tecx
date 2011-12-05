namespace TecX.Search.Test.TestObjects
{
    using System;
    using System.Data.Common;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    class MockDatabase : Database
    {
        public MockDatabase(string connectionString, DbProviderFactory dbProviderFactory)
            : base(connectionString, dbProviderFactory)
        {
        }

        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            throw new NotImplementedException();
        }
    }
}