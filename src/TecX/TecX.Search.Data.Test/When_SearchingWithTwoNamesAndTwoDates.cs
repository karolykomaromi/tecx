namespace TecX.Search.Data.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class When_SearchingWithTwoNamesAndTwoDates : Given_DynamicSearchPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            this.searchParameters.Add(new SearchParameter<string>("aa"));
            this.searchParameters.Add(new SearchParameter<string>("mfs"));
            this.searchParameters.Add(new SearchParameter<DateTime>(new DateTime(2011, 11, 13)));
            this.searchParameters.Add(new SearchParameter<DateTime>(new DateTime(2011, 11, 13, 23, 00, 00)));
        }

        [TestMethod]
        public void Then_CreatesDynamicQuery()
        {
            Assert.AreEqual(1, this.result.Count);
            Assert.AreSame(this.cust1, result[0]);
        }
    }
}