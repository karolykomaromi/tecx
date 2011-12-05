namespace TecX.Search.Data.Test
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ploeh.AutoFixture;

    using TecX.Logging;
    using TecX.Search;
    using TecX.Search.Data.EF;
    using TecX.Search.Data.EF.Reader;
    using TecX.Search.Data.Test.TestObjects;
    using TecX.Search.Model;

    [TestClass]
    public class MessageTestDataGeneration
    {
        [TestMethod]
        [Ignore]
        public void GenerateAndSafeData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MessageSearch"].ConnectionString;
            
            int count = 50000;

            var fixture = new Fixture();

            fixture.Customizations.Add(new CurrentDateTimeGenerator());
            fixture.Customizations.Add(new MessagePriorityGenerator());

            List<Message> messages = new List<Message>(count);

            for (int i = 0; i < count; i++)
            {
                var msg = fixture.CreateAnonymous<Message>();

                messages.Add(msg);
            }

            var reader = messages.AsDataReader();

            SqlBulkCopy copy = new SqlBulkCopy(connectionString);

            copy.ColumnMappings.Add("Id", "id");

            copy.DestinationTableName = "Messages";

            copy.WriteToServer(reader);
        }
    }

    [TestClass]
    public class FullTextSearchTermsFixture
    {
        [TestMethod]
        [Ignore]
        public void CanAddSearchTermsUsingTableValuedParameter()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MessageSearch"].ConnectionString;

            var entities = new MessageEntities(connectionString);

            var searchTerms = new[]
                {
                    new SearchTerm { MessageId = 1, Text = "MFS" }, 
                    new SearchTerm { MessageId = 1, Text = "TENG" },
                    new SearchTerm { MessageId = 9, Text = "VLAN" }, 
                    new SearchTerm { MessageId = 9, Text = "MFS" }
                };

            entities.AddSearchTerms(searchTerms);
        }
    }

    [TestClass]
    public class FullTextProcessingFixture
    {
        [TestMethod]
        [Ignore]
        public void CanProcessALotOfMessages()
        {
            using (new TraceTimer("FullTextIndexing"))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MessageSearch"].ConnectionString;

                var entities = new MessageEntities(connectionString);

                ((IObjectContextAdapter)entities).ObjectContext.CommandTimeout = 360;

                var repository = new EFMessageRepository(entities, new FullTextSearchTermProcessor());

                repository.IndexMessagesForFullTextSearch();
            }
        }
    }
}
