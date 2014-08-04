namespace TecX.Search.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using TecX.Search;
    using TecX.Search.Data.EF;
    using TecX.Search.Data.Test.TestObjects;
    using TecX.Search.Model;
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

        protected Message msg0;

        protected Message msg1;

        protected Message msg2;

        protected Message msg3;

        protected override void Given()
        {
            this.msg0 = new Message { Source = "mfs123", SentAt = new DateTime(2011, 11, 10), Id = 0 };
            this.msg1 = new Message { Source = "aabc", SentAt = new DateTime(2011, 11, 13), Id = 1 };
            this.msg2 = new Message { Source = "mfs123", SentAt = new DateTime(2011, 11, 14), Id = 2 };
            this.msg3 = new Message { Source = "123", SentAt = new DateTime(2011, 11, 13), Id = 3 };

            this.set = new InMemoryDbSet<Message> { this.msg0, this.msg1, this.msg2, this.msg3 };

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