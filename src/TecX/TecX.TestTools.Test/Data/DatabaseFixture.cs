namespace TecX.TestTools.Test.Data
{
    using TestTools.Data;

    using Xunit;

    public class DatabaseFixture
    {
        [Fact]
        public void Should_NotThrowExceptionOnCreation()
        {
            using (var result = Database.Build(
                x =>
                    {
                        x.WithConnectionStringNamed("MyConnectionString");
                        x.WithDatabaseName("FooDatabase");

                        x.BuildSequence(
                            db =>
                                {
                                    db.DropExistingDatabase();
                                    db.CreateEmptyDatabase();
                                    db.CreateTables();
                                    db.CreateTestData();
                                });
                    }))
            {
                Assert.Null(result.Error);
            }
        }
    }
}
