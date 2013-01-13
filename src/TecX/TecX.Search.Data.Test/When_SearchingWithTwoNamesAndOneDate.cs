namespace TecX.Search.Data.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class When_SearchingWithTwoNamesAndOneDate : Given_DynamicSearchPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            this.searchParameters.Add(new SearchParameter<string>("aa"));
            this.searchParameters.Add(new SearchParameter<string>("mfs"));
            this.searchParameters.Add(new SearchParameter<DateTime>(new DateTime(2011, 11, 11)));
        }

        [TestMethod]
        public void Then_CreatesDynamicQuery()
        {
            Assert.AreEqual(2, this.result.Count);
            Assert.AreEqual(2, this.result[0].Id);
            Assert.AreEqual(1, this.result[1].Id);
        }
    }
}