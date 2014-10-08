namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    [TestClass]
    public class DamerauLevenshteinDistanceFixture
    {
        [TestMethod]
        public void Should_CaculateDistanceBetween_teusday_And_tuesday_As_1()
        {
            string s = "teusday";
            string t = "tuesday";

            var algorithm = new DamerauLevenshteinDistance();

            Assert.AreEqual(1, algorithm.GetDamerauLevenshteinDistance(s, t));
        }

        [TestMethod]
        public void Should_CaculateDistanceBetween_teusday_And_thursday_As_2()
        {
            string s = "teusday";
            string t = "thursday";

            var algorithm = new DamerauLevenshteinDistance();

            Assert.AreEqual(2, algorithm.GetDamerauLevenshteinDistance(s, t));
        }

        [TestMethod]
        public void Should_CaculateDistanceBetween_tuesday_And_something_As_8()
        {
            string s = "tuesday";
            string t = "something";

            var algorithm = new DamerauLevenshteinDistance();

            Assert.AreEqual(8, algorithm.GetDamerauLevenshteinDistance(s, t));
        }

        [TestMethod]
        public void Should_CaculateDistanceBetween_CA_And_ABC_As_2()
        {
            string s = "CA";
            string t = "ABC";

            var algorithm = new DamerauLevenshteinDistance();

            Assert.AreEqual(2, algorithm.GetDamerauLevenshteinDistance(s, t));
        }
    }
}
