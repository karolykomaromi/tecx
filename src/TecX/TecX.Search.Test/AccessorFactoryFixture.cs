namespace TecX.Search.Test
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;
    using TecX.Search.Data.EntLib;
    using TecX.Search.Test.TestObjects;
    using TecX.TestTools;

    using Constants = TecX.Search.Data.EntLib.Constants;

    public abstract class Given_AccessorFactoryPrerequisites : GivenWhenThen
    {
        protected Database db;

        protected SearchParameterCollection searchParameters;

        protected AccessorFactory factory;

        protected  CustomSprocAccessor<Message> accessor;

        protected override void Given()
        {
            this.db = new MockDatabase("abc", new MockDbProviderFactory());

            this.searchParameters = new SearchParameterCollection();

            this.factory = new AccessorFactory();
        }

        protected override void When()
        {
            this.accessor = this.factory.CreateAccessor(this.db, this.searchParameters);
        }
    }

    [TestClass]
    public class When_CreatingAccessorForNameAndTimeSearch : Given_AccessorFactoryPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            searchParameters.Add(new SearchParameter<DateTime>(DateTime.MinValue));
            searchParameters.Add(new SearchParameter<string>("abc"));
        }

        [TestMethod]
        public void Then_FactoryCreatesAccessorForNameAndTimeSearch()
        {
            Assert.AreEqual(Constants.ProcedureNames.SearchByInterfaceAndDate, this.accessor.ProcedureName);
        }
    }

    [TestClass]
    public class When_CreatingAccessorForNameAndTimeFrameSearch : Given_AccessorFactoryPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            searchParameters.Add(new SearchParameter<DateTime>(DateTime.MinValue));
            searchParameters.Add(new SearchParameter<DateTime>(DateTime.MaxValue));
            searchParameters.Add(new SearchParameter<string>("abc"));
        }

        [TestMethod]
        public void Then_FactoryCreatesAccessorForNameAndTimeFrameSearch()
        {
            Assert.AreEqual(Constants.ProcedureNames.SearchByInterfaceAndTimeFrame, this.accessor.ProcedureName);
        }
    }

    [TestClass]
    public class When_CreatingAccessorForTimeFrameSearch : Given_AccessorFactoryPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            searchParameters.Add(new SearchParameter<DateTime>(DateTime.MinValue));
            searchParameters.Add(new SearchParameter<DateTime>(DateTime.MaxValue));
        }

        [TestMethod]
        public void Then_FactroyCreatesAccessorForNameAndTimeFrameSearch()
        {
            Assert.AreEqual(Constants.ProcedureNames.SearchByTimeFrame, this.accessor.ProcedureName);
        }
    }

    [TestClass]
    public class AccessorFactoryFixture
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ThrowsOnUnknownParameterCombination()
        {
            new AccessorFactory().CreateAccessor(new MockDatabase("abc", new MockDbProviderFactory()), new SearchParameterCollection());
        }
    }
}
