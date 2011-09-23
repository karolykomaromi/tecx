using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Measures.Test
{
    [TestClass]
    public class WeightFixture
    {
        [TestMethod]
        public void CanGetWeightFromTicks()
        {
            Weight weight = Weight.FromTicks(10000);

            Assert.AreEqual(10000, weight.Ticks);
        }

        [TestMethod]
        public void CanGetWeightFromMilligrams()
        {
            Weight weight = Weight.FromMilligrams(1.0);

            Assert.AreEqual(1.0, weight.Milligrams);
        }

        [TestMethod]
        public void CanGetWeightFromGrams()
        {
            Weight weight = Weight.FromGrams(1.0);

            Assert.AreEqual(1.0, weight.Grams);
        }

        [TestMethod]
        public void CanGetWeightFromKilograms()
        {
            Weight weight = Weight.FromKilograms(1.0);

            Assert.AreEqual(1.0, weight.Kilograms);
        }

        [TestMethod]
        public void CanGetWeightFromTons()
        {
            Weight weight = Weight.FromTons(1.0);

            Assert.AreEqual(1.0, weight.Tons);
        }

        [TestMethod]
        public void CanGetTotalMilligrams()
        {
            Weight weight = new Weight(0, 0, 10, 500);

            Assert.AreEqual(10500, weight.TotalMilligrams);
        }

        [TestMethod]
        public void CanGetTotalGrams()
        {
            Weight weight = new Weight(0, 10, 500, 0);

            Assert.AreEqual(10500, weight.TotalGrams);
        }

        [TestMethod]
        public void CanGetTotalKilograms()
        {
            Weight weight = new Weight(10, 500, 0, 0);

            Assert.AreEqual(10500, weight.TotalKilograms);
        }

        [TestMethod]
        public void CanGetTotalTons()
        {
            Weight weight = new Weight(1, 500, 0, 0);

            Assert.AreEqual(1.5, weight.TotalTons);
        }
    }
}
