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

            Assert.AreEqual(1.Kilometers(), km1 + km2);
        }

        [TestMethod]
        public void CanDivideByTime()
        {
            var distance = 60.Kilometers();
            var time = 2.Hours();

            Assert.AreEqual(30.Kmh(), distance / time);
        }
    }
}
