using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Extensions.Primitives;

namespace TecX.Common.Measures.Test
{
    [TestClass]
    public class KilometersPerHourFixture
    {
        [TestMethod]
        public void CanCompareByEqualsKmh()
        {
            KilometersPerHour kmh1 = 2.0;
            KilometersPerHour kmh2 = 2.0;

            Assert.IsTrue(kmh1 == kmh2);
        }

        [TestMethod]
        public void CanCompareByNotEqualsKmh()
        {
            KilometersPerHour kmh1 = 2.0;
            KilometersPerHour kmh2 = 3.0;

            Assert.IsTrue(kmh1 != kmh2);
        }

        [TestMethod]
        public void CanAddKmh()
        {
            KilometersPerHour kmh1 = 2.0;
            KilometersPerHour kmh2 = 4.0;

            KilometersPerHour kmhSum = 6.0;

            Assert.AreEqual(kmhSum, kmh1 + kmh2);
        }

        [TestMethod]
        public void CanSubtractKmh()
        {
            KilometersPerHour kmh1 = 4.0;
            KilometersPerHour kmh2 = 3.0;

            KilometersPerHour kmhSum = 1.0;

            Assert.AreEqual(kmhSum, kmh1 - kmh2);
        }

        [TestMethod]
        public void CanCreateByDivision()
        {
            double distance = 60.0;
            TimeSpan time = 2.Hours();

            KilometersPerHour kmh = KilometersPerHour.FromDistanceAndTime(distance, time);

            Assert.AreEqual(30.Kmh(), kmh);
        }

        [TestMethod]
        public void CanUseExtensionOrImplicitOperator()
        {
            var kmh1 = 2.Kmh();
            KilometersPerHour kmh2 = 2.0;

            Assert.AreEqual(kmh1, kmh2);
        }

        [TestMethod]
        public void CanMultiplyWithTime()
        {
            KilometersPerHour kmh = 30.0;
            TimeSpan time = 2.Hours();

            Kilometer distance = kmh * time;

            Assert.AreEqual(60.Kilometers(), distance);
        }

        [TestMethod]
        public void CanCastExplicitely()
        {
            var speed = 100.Kmh();

            Assert.AreEqual(100.0, (double)speed);
        }

        [TestMethod]
        public void CanAddDouble()
        {
            var speed = 100.Kmh();

            Assert.AreEqual(110.Kmh(), speed + 10.0);
        }

        [TestMethod]
        public void CanSubtractDouble()
        {
            var speed = 100.Kmh();

            Assert.AreEqual(90.Kmh(), speed - 10.0);
        }
    }
}
