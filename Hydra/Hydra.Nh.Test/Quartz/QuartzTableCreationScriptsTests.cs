namespace Hydra.Nh.Test.Quartz
{
    using System;
    using Hydra.Nh.Quartz;
    using Xunit;

    public class QuartzTableCreationScriptsTests
    {
        [Fact]
        public void Should_Get_MySql_Script()
        {
            string actual = QuartzTableCreationScripts.MySql.Script;
            Assert.NotNull(actual);
            Assert.Contains("mysql", actual, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_Oracle_Script()
        {
            string actual = QuartzTableCreationScripts.Oracle.Script;
            Assert.NotNull(actual);
            Assert.Contains("oracle", actual, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_PostgreSql_Script()
        {
            string actual = QuartzTableCreationScripts.PostgreSql.Script;
            Assert.NotNull(actual);
            Assert.Contains("postgresql", actual, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_SqlServer_Script()
        {
            string actual = QuartzTableCreationScripts.SqlServer.Script;
            Assert.NotNull(actual);
            Assert.Contains("sql server", actual, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_Sqlite_Script()
        {
            string actual = QuartzTableCreationScripts.Sqlite.Script;
            Assert.NotNull(actual);
            Assert.Contains("sqlite", actual, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_SqlServerCe_Script()
        {
            string actual = QuartzTableCreationScripts.SqlServerCe.Script;
            Assert.NotNull(actual);
            Assert.Contains("sql server ce", actual, StringComparison.OrdinalIgnoreCase);
        }
    }
}
