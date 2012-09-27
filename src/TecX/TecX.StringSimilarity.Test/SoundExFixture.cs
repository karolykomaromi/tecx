namespace TecX.StringSimilarity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.StringSimilarity;

    [TestClass]
    public class SoundExFixture
    {
        [TestMethod]
        public void Should_BeEqual_AccordingTo_SoundEx()
        {
            string s = "Bewährten Superzicke";
            string t = "Britney Spears";

            var algorithm = new SoundEx();

            Assert.AreEqual(1, algorithm.GetSimilarity(s, t));
        }

        [TestMethod]
        public void Should_Encode_Rupert_As_R163()
        {
            string s = "Rupert";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("R163", encoded);
        }

        [TestMethod]
        public void Should_Encode_Robert_As_R163()
        {
            string s = "Robert";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("R163", encoded);
        }

        [TestMethod]
        public void Should_Encode_Rubin_As_R150()
        {
            string s = "Rubin";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("R150", encoded);
        }

        [TestMethod]
        public void Should_Encode_Ashcraft_As_A226()
        {
            string s = "Ashcraft";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("A226", encoded);
        }

        [TestMethod]
        public void Should_Encode_Ashcroft_As_A226()
        {
            string s = "Ashcroft";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("A226", encoded);
        }

        [TestMethod]
        public void Should_Encode_Tymczak_As_T522()
        {
            string s = "Tymczak_";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("T522", encoded);
        }

        [TestMethod]
        public void Should_Encode_Pfister_As_P123()
        {
            string s = "Pfister";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual("P123", encoded);
        }

        [TestMethod]
        public void Should_Encode_BritneySpears_As_B635()
        {
            string s = "Britney Spears";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual(encoded, "B635");
        }

        [TestMethod]
        public void Should_Encode_BewaehrtenSuperzicke_As_B635()
        {
            string s = "Bewährten Superzicke";

            var algorithm = new SoundEx();

            string encoded = algorithm.GetSoundEx(s);

            Assert.AreEqual(encoded, "B635");
        }
    }
}
