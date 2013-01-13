namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class HasCtorWithConnectionString
    {
        public string ConnectionString { get; set; }

        public HasCtorWithConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}