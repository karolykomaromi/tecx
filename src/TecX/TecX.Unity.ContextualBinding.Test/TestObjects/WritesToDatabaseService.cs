namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    public class WritesToDatabaseService : IMyService
    {
        public string ConnectionString { get; set; }

        public string ConnectionString2 { get; set; }

        public WritesToDatabaseService(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}