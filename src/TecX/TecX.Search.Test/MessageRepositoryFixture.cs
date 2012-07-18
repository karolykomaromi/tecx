namespace TecX.Search.Test
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;
    using TecX.Search.Data.EntLib;
    using TecX.Search.Model;

    using Constants = TecX.Search.Data.EntLib.Constants;

    [TestClass]
    public class MessageRepositoryFixture
    {
        [TestMethod]
        [Ignore]
        public void CanSearchUsingEntLib()
        {
            var repository = new EntLibMessageRepository(new SqlDatabase(Constants.ConnectionString), new AccessorFactory());

            var searchParameters = new SearchParameterCollection
                {
                    new SearchParameter<string>("%msf%"),
                    new SearchParameter<DateTime>(new DateTime(2011, 1, 1))
                    //,new SearchParameter<DateTime>(new DateTime(2012, 1, 1))
                };

            int total;

            var searchResult = repository.Search(100, out total, searchParameters);

            foreach (Message message in searchResult)
            {
                Console.WriteLine(message);
            }
        }

        [TestMethod]
        [Ignore]
        public void CustomAccessorWithOutputParameter()
        {
            var rowMapper = MapBuilder<Message>.MapAllProperties().Build();

            var db = new SqlDatabase(Constants.ConnectionString);

            int totalRowsCount = 0;

            var accessor = new CustomSprocAccessor<Message>(db, "WithOutputParameter", rowMapper);

            var messages = accessor.Execute(totalRowsCount);

            foreach (var msg in messages)
            {
                Console.WriteLine(msg);
            }

            Console.WriteLine(accessor.TotalRowCount + " total.");
        }
    }
}