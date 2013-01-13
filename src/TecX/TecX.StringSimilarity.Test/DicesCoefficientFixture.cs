namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    [TestClass]
    public class DicesCoefficientFixture
    {
        [TestMethod]
        public void Should_CalculateSimilarity_Between_Night_And_Nacht_As_25_Percent()
        {
            string s = "night";
            string t = "nacht";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.25, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_France_And_french_As_40_Percent()
        {
            string s = "France";
            string t = "french";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.4, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Sealed_As_80_Percent()
        {
            string s = "Healed";
            string t = "Sealed";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.8, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Healthy_As_55_Percent()
        {
            string s = "Healed";
            string t = "Healthy";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.55, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Heard_As_44_Percent()
        {
            string s = "Healed";
            string t = "Heard";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.44, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Herded_As_40_Percent()
        {
            string s = "Healed";
            string t = "Herded";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.40, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Help_As_25_Percent()
        {
            string s = "Healed";
            string t = "Help";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.25, similarity);
        }

        [TestMethod]
        public void Should_CalculateSimilarity_Between_Healed_And_Sold_As_0_Percent()
        {
            string s = "Healed";
            string t = "Sold";

            var algorithm = new DicesCoefficient();

            double similarity = algorithm.GetDicesCoefficient(s, t);

            Assert.AreEqual(0.0, similarity);
        }
    }
}
