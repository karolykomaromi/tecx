namespace TecX.Search.Data.Test
{
    using System;
    using System.Data.SqlTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TecX.Search;
    using TecX.Search.Data.EF;
    using TecX.Search.Data.Test.TestObjects;

    [TestClass]
    public class EFMessageRepositoryFixture
    {
        [TestMethod]
        public void CallsSearchByInterfaceAndTimeFrame()
        {
            var parameters = new SearchParameterCollection
                {
                    new SearchParameter<string>("1"),
                    new SearchParameter<DateTime>(DateTime.MinValue),
                    new SearchParameter<DateTime>(DateTime.MaxValue)
                };

            var mock = new Mock<IMessageEntities>();

            int totalRowsCountDummy;

            mock.Setup(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", DateTime.MinValue, DateTime.MaxValue))
                .Returns(new Message[0]);

            var repository = new EFMessageRepository(mock.Object, new NullFullTextSearchTermProcessor());

            int totalRowsCount;

            var result = repository.Search(100, out totalRowsCount, parameters);

            mock.Verify(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", DateTime.MinValue, DateTime.MaxValue), Times.Once());

            Assert.IsNotNull(result as Message[]);
            Assert.AreEqual(0, totalRowsCount);
        }

        [TestMethod]
        public void CallsSearchByTimeFrame()
        {
            var parameters = new SearchParameterCollection
                {
                    new SearchParameter<DateTime>(DateTime.MinValue),
                    new SearchParameter<DateTime>(DateTime.MaxValue)
                };

            var mock = new Mock<IMessageEntities>();

            int totalRowsCountDummy;

            mock.Setup(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "*", DateTime.MinValue, DateTime.MaxValue))
                .Returns(new Message[0]);

            var repository = new EFMessageRepository(mock.Object, new NullFullTextSearchTermProcessor());

            int totalRowsCount;

            var result = repository.Search(100, out totalRowsCount, parameters);

            mock.Verify(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "*", DateTime.MinValue, DateTime.MaxValue), Times.Once());

            Assert.IsNotNull(result as Message[]);
            Assert.AreEqual(0, totalRowsCount);
        }

        [TestMethod]
        public void CallsSearchByInterfaceAndDate()
        {
            DateTime referenceDate = new DateTime(2011, 11, 11);

            var parameters = new SearchParameterCollection
                {
                    new SearchParameter<string>("1"),
                    new SearchParameter<DateTime>(referenceDate)
                };

            var mock = new Mock<IMessageEntities>();

            int totalRowsCountDummy;

            mock.Setup(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", referenceDate, SqlDateTime.MaxValue.Value))
                .Returns(new Message[0]);

            var repository = new EFMessageRepository(mock.Object, new NullFullTextSearchTermProcessor());

            int totalRowsCount;

            var result = repository.Search(100, out totalRowsCount, parameters);

            mock.Verify(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", referenceDate, SqlDateTime.MaxValue.Value), Times.Once());

            Assert.IsNotNull(result as Message[]);
            Assert.AreEqual(0, totalRowsCount);
        }

        [TestMethod]
        public void CallsSearchByInterface()
        {
            var parameters = new SearchParameterCollection
                {
                    new SearchParameter<string>("1")
                };

            var mock = new Mock<IMessageEntities>();

            int totalRowsCountDummy;

            mock.Setup(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value))
                .Returns(new Message[0]);

            var repository = new EFMessageRepository(mock.Object, new NullFullTextSearchTermProcessor());

            int totalRowsCount;

            var result = repository.Search(100, out totalRowsCount, parameters);

            mock.Verify(ase => ase.SearchByInterfaceAndTimeFrame(It.IsAny<int>(), out totalRowsCountDummy, "1", SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value), Times.Once());

            Assert.IsNotNull(result as Message[]);
            Assert.AreEqual(0, totalRowsCount);
        }
    }
}
