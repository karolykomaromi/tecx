namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    [TestClass]
    public class JaccardCoefficientFixture
    {
        private static double Epsilon = 0.001;

        [TestMethod]
        public void Should_CalculateSimilarity_BetweenTotallyUnrelatedAddresses_AsVeryLow()
        {
            string s = "Burra Hotel, 5 Market Sq, Burra, SA, 5417";
            string t = "Camping Country Superstore, 401 Pacific Hwy, Belmont North, NSW, 2280";

            var algorithm = new JaccardIndex();

            double coefficient = algorithm.GetJaccardIndex(s, t);

            Assert.AreEqual(0.068, coefficient, Epsilon);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_BetweenAddresses_WithAndWithout_Abrevations_AsHigh()
        {
            string s = "One Stop Bakery, 1304 High St Rd, Wantirna, VIC, 3152";
            string t = "One Stop Bakery, 1304 High Street Rd, Wantirna South, VIC, 3152";

            var algorithm = new JaccardIndex();

            double coefficient = algorithm.GetJaccardIndex(s, t);

            Assert.AreEqual(0.807, coefficient, Epsilon);
        }

        [TestMethod]
        public void Should_CalculateSimilarityBetween_AddressesWithDifferentStreetNumbers_AsHigh()
        {
            string s = "Weaver Interiors, 955 Pacific Hwy, Pymble, NSW, 2073";
            string t = "Weaver Interiors, 997 Pacific Hwy, Pymble, NSW, 2073";

            var algorithm = new JaccardIndex();

            double coefficient = algorithm.GetJaccardIndex(s, t);

            Assert.AreEqual(0.877, coefficient, Epsilon);
        }

        [TestMethod]
        public void Should_CalculateSimilarityBetween_AddressesWithDifferent_InhabitantNames_AsMedium()
        {
            string s = "Gibbon Hamor Commercial Interiors, 233 Johnston St, Annandale, NSW, 2038";
            string t = "Gibbon Hamor Development Planners, 233 Johnston St, Annandale, NSW, 2038";

            var algorithm = new JaccardIndex();

            double coefficient = algorithm.GetJaccardIndex(s, t);

            Assert.AreEqual(0.644, coefficient, Epsilon);
        }
    }
}
