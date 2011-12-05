namespace TecX.Search.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using TecX.Search;
    using TecX.Search.Data.EF;
    using TecX.Search.Data.Test.TestObjects;
    using TecX.Search.Test.TestObjects;
    using TecX.TestTools;

    public abstract class Given_DynamicSearchPrerequisites : GivenWhenThen
    {
        protected InMemoryDbSet<Message> set;

        protected Mock<IMessageEntities> mock;

        protected EFMessageRepository repository;

        protected string pattern;

        protected SearchParameterCollection searchParameters;

        protected int totalRowsCount;

        protected List<Message> result;

        protected Message cust0;

        protected Message cust1;

        protected Message cust2;

        protected Message cust3;

        protected override void Given()
        {
            this.cust0 = new Message { Description = "mfs123", SentAt = new DateTime(2011, 11, 10), Id = 0 };
            this.cust1 = new Message { Description = "aabc", SentAt = new DateTime(2011, 11, 13), Id = 1 };
            this.cust2 = new Message { Description = "mfs123", SentAt = new DateTime(2011, 11, 14), Id = 2 };
            this.cust3 = new Message { Description = "123", SentAt = new DateTime(2011, 11, 13), Id = 3 };

            this.set = new InMemoryDbSet<Message> { this.cust0, this.cust1, this.cust2, this.cust3 };

            this.mock = new Mock<IMessageEntities>();

            this.mock.SetupGet(entities => entities.Messages).Returns(this.set);

            this.repository = new EFMessageRepository(this.mock.Object, new NullFullTextSearchTermProcessor());

            this.searchParameters = new SearchParameterCollection();
        }

        protected override void When()
        {
            this.result = this.repository.Search(100, out this.totalRowsCount, this.searchParameters).ToList();
        }
    }
}