using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Measures.Test
{
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
    }
}
