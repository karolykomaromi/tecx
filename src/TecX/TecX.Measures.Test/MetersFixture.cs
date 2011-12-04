namespace TecX.Measures.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Measures;

    [TestClass]
    public class MetersFixture
    {
        [TestMethod]
        public void CanAddMeters()
        {
            var d1 = 1.5.Meters();
            Meter d2 = 4;

            var d3 = 5.5.Meters();

            Assert.AreEqual(d3, d1 + d2);
        }

        [TestMethod]
        public void CanSubtractMeters()
        {
            Meter d1 = 4;
            Meter d2 = 1.5;

            Meter d3 = 2.5;

            Assert.AreEqual(d3, d1 - d2);
        }

        [TestMethod]
        public void CanConvertToKilometers()
        {
            Meter d1 = 750.Meters();

            Kilometer d2 = d1.ToKilometers();

            Assert.AreEqual(0.75.Kilometers(), d2);
        }

        [TestMethod]
        public void CanCastExplicitely()
        {
            Meter d1 = 100.Meters();

            Assert.AreEqual(100.0, (double)d1);
        }

        [TestMethod]
        public void CanAddDouble()
        {
            Meter d1 = 100.Meters();

            Assert.AreEqual(200.Meters(), d1 + 100.0);
        }

        [TestMethod]
        public void CanSubtractDouble()
        {
            Meter d1 = 150.0;

            Assert.AreEqual(50.Meters(), d1 - 100.0);
        }
    }
}
