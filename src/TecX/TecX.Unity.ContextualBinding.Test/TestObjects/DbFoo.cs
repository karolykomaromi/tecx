namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    public class DbFoo : IFoo
    {
        public DbFoo(string connectionString, string connectionString2)
        {
            this.ConnectionString = connectionString;
            this.ConnectionString2 = connectionString2;
        }

        public string ConnectionString { get; set; }

        public string ConnectionString2 { get; set; }
    }
}