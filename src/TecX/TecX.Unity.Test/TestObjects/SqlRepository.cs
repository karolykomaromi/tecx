using TecX.Common;

namespace TecX.Unity.Test.TestObjects
{
    public class SqlRepository
    {
        public string ConnectionString { get; private set; }

        public SqlRepository(string connectionString)
        {
            Guard.AssertNotNull(connectionString, "connectionString");

            ConnectionString = connectionString;
        }
    }
}