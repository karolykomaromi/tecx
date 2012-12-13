using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.TestTools.Test.Data
{
    using TecX.TestTools.Data;

    using Xunit;

    public class DatabaseFixture
    {
        [Fact]
        public void Should()
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
