namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    [TestClass]
    public class HammingDistanceFixture
    {
        [TestMethod]
        public void Should_CalculateDistanceBetween_toned_And_roses_As_3()
        {
            string s = "toned";
            string t = "roses";

            var algorithm = new HammingDistance();

            Assert.AreEqual(3, algorithm.GetHammingDistance(s, t));
        }

        [TestMethod]
        public void Should_CalculateDistanceBetween_1011101_And_1001001_As_2()
        {
            string s = "1011101";
            string t = "1001001";

            var algorithm = new HammingDistance();

            Assert.AreEqual(2, algorithm.GetHammingDistance(s, t));
        }

        [TestMethod]
        public void Should_CalculateDistanceBetween_2173896_And_2233796_As_3()
        {
            string s = "2173896";
            string t = "2233796";

            var algorithm = new HammingDistance();

            Assert.AreEqual(3, algorithm.GetHammingDistance(s, t));
        }
    }
}
