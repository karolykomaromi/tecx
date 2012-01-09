namespace TecX.Search.Data.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class When_SearchingWithTwoNames : Given_DynamicSearchPrerequisites
    {
        protected override void Given()
        {
            base.Given();

            this.searchParameters.Add(new SearchParameter<string>("aa"));
            this.searchParameters.Add(new SearchParameter<string>("mfs"));
        }

        [TestMethod]
        public void Then_CreatesDynamicQuery()
        {
            Assert.AreEqual(3, this.result.Count);
            Assert.AreSame(this.msg2, result[0]);
            Assert.AreSame(this.msg1, result[1]);
            Assert.AreSame(this.msg0, result[2]);
        }
    }
}