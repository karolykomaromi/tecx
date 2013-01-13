namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    /// <summary>
    /// Samples taken from http://www.miislita.com/searchito/levenshtein-edit-distance.html
    /// </summary>
    [TestClass]
    public class LevenshteinDistanceFixture
    {
        [TestMethod]
        public void DistanceBetween_Kitten_And_Sitting_Should_Be_3()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Kitten", "Sitting");

            Assert.AreEqual(3, distance);
        }

        [TestMethod]
        public void DistanceBetween_Democrats_And_Republicans_Should_Be_8()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Democrats", "Republicans");

            Assert.AreEqual(8, distance);
        }

        [TestMethod]
        public void DistanceBetween_Google_And_Yahoo_Should_Be_6()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Google", "Yahoo!");

            Assert.AreEqual(6, distance);
        }

        [TestMethod]
        public void DistanceBetween_Good_And_Evil_Should_Be_4()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Good", "Evil");

            Assert.AreEqual(4, distance);
        }

        [TestMethod]
        public void DistanceBetween_password_And_userID_Should_Be_7()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("password", "userID");

            Assert.AreEqual(7, distance);
        }

        [TestMethod]
        public void DistanceBetween_Jesus_And_Satan_Should_Be_5()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Jesus", "Satan");

            Assert.AreEqual(5, distance);
        }

        [TestMethod]
        public void DistanceBetween_Britney_And_Spears_Should_Be_7()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Britney", "Spears");

            Assert.AreEqual(7, distance);
        }

        [TestMethod]
        public void DistanceBetween_LottoNo_And_QuickPickNo_Should_Be_10()
        {
            var algorithm = new LevenshteinDistance();

            int distance = algorithm.GetLevenshteinDistance("Lotto No.", "Quick Pick No.");

            Assert.AreEqual(10, distance);
        }
    }
}