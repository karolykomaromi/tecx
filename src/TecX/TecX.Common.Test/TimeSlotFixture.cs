using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Time;

namespace TecX.Common.Test
{
    [TestClass]
    public class TimeSlotFixture
    {
        [TestMethod]
        public void ContainsExcludeBounds()
        {
            TimeSlot slot = new TimeSlot(new DateTime(2010, 7, 1), new DateTime(2010, 7, 5) );

            DateTime inside = new DateTime(2010, 7, 3);

            Assert.IsTrue(slot.Contains(inside));

            DateTime lowerBound = new DateTime(2010, 7, 1);

            Assert.IsFalse(slot.Contains(lowerBound));

            DateTime upperBound = new DateTime(2010, 7, 5);

            Assert.IsFalse(slot.Contains(upperBound));

            DateTime wellOutside = new DateTime(2008, 7, 1);

            Assert.IsFalse(slot.Contains(wellOutside));

        }

        [TestMethod]
        public void ContainsIncludeBounds()
        {
            TimeSlot slot = new TimeSlot(new DateTime(2010, 7, 1), new DateTime(2010, 7, 5));

            DateTime inside = new DateTime(2010, 7, 3);

            Assert.IsTrue(slot.Contains(inside, IncludeOptions.Both));

            DateTime lowerBound = new DateTime(2010, 7, 1);

            Assert.IsTrue(slot.Contains(lowerBound, IncludeOptions.Both));

            DateTime upperBound = new DateTime(2010, 7, 5);

            Assert.IsTrue(slot.Contains(upperBound, IncludeOptions.Both));

            DateTime wellOutside = new DateTime(2008, 7, 1);

            Assert.IsFalse(slot.Contains(wellOutside, IncludeOptions.Both));
        }

        [TestMethod]
        public void ContainsIncludeBegin()
        {
            TimeSlot slot = new TimeSlot(new DateTime(2010, 7, 1), new DateTime(2010, 7, 5));

            DateTime inside = new DateTime(2010, 7, 3);

            Assert.IsTrue(slot.Contains(inside, IncludeOptions.Begin));

            DateTime lowerBound = new DateTime(2010, 7, 1);

            Assert.IsTrue(slot.Contains(lowerBound, IncludeOptions.Begin));

            DateTime upperBound = new DateTime(2010, 7, 5);

            Assert.IsFalse(slot.Contains(upperBound, IncludeOptions.Begin));

            DateTime wellOutside = new DateTime(2008, 7, 1);

            Assert.IsFalse(slot.Contains(wellOutside, IncludeOptions.Begin));
        }

        [TestMethod]
        public void ContainsIncludeEnd()
        {
            TimeSlot slot = new TimeSlot(new DateTime(2010, 7, 1), new DateTime(2010, 7, 5));

            DateTime inside = new DateTime(2010, 7, 3);

            Assert.IsTrue(slot.Contains(inside, IncludeOptions.End));

            DateTime lowerBound = new DateTime(2010, 7, 1);

            Assert.IsFalse(slot.Contains(lowerBound, IncludeOptions.End));

            DateTime upperBound = new DateTime(2010, 7, 5);

            Assert.IsTrue(slot.Contains(upperBound, IncludeOptions.End));

            DateTime wellOutside = new DateTime(2008, 7, 1);

            Assert.IsFalse(slot.Contains(wellOutside, IncludeOptions.End));
        }
    }
}
