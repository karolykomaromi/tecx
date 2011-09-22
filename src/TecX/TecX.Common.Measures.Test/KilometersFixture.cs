using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Extensions.Primitives;

namespace TecX.Common.Measures.Test
{
    [TestClass]
    public class KilometersFixture
    {
        [TestMethod]
        public void CanAddKilometers()
        {
            var km1 = 5.Kilometers();
            var km2 = 3.0.Kilometers();

            Assert.AreEqual(8.Kilometers(), km1 + km2);
        }

        [TestMethod]
        public void CanSubtractKilometers()
        {
            var km1 = 4.Kilometers();
            var km2 = 3.Kilometers();

            Assert.AreEqual(1.Kilometers(), km1 - km2);
        }

        [TestMethod]
        public void CanDivideByTime()
        {
            var distance = 60.Kilometers();
            var time = 2.Hours();

            Assert.AreEqual(30.Kmh(), distance / time);
        }

        [TestMethod]
        public void CanConvertToMeters()
        {
            Kilometer d1 = 1.0;

            Assert.AreEqual(1000.Meters(), d1.ToMeters());
        }

        [TestMethod]
        public void CanTestEquality()
        {
            Assert.IsTrue(1.Kilometers() == 1.Kilometers());
        }

        [TestMethod]
        public void CanTestNonEquality()
        {
            Assert.IsTrue(1.Kilometers() != 2.Kilometers());
        }

        [TestMethod]
        public void CanCastExplicitely()
        {
            var d1 = 1.5.Kilometers();

            Assert.AreEqual(1.5, (double)d1);
        }

        [TestMethod]
        public void CanSubtractMeters()
        {
            var d1 = 1.1.Kilometers();
            var d2 = 100.Meters();

            Assert.AreEqual(1.Kilometers(), d1 - d2);
        }

        [TestMethod]
        public void CanSubtractDouble()
        {
            Kilometer d1 = 1.1.Kilometers();
            double d2 = 0.1;

            Assert.AreEqual(1.Kilometers(), d1 - d2);
        }

        [TestMethod]
        public void CanAddMeters()
        {
            Kilometer d1 = 1.Kilometers();

            Meter d2 = 100.Meters();

            Assert.AreEqual(1.1.Kilometers(), d1 + d2);
        }

        [TestMethod]
        public void CanAddDouble()
        {
            Kilometer d1 = 1.4.Kilometers();

            Assert.AreEqual(1.5.Kilometers(), d1 + 0.1);
        }
    }
}
