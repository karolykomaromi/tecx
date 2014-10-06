namespace TecX.Common.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Error;

    [TestClass]
    public class GuardFixture
    {
        [TestMethod]
        public void CanAssertArgumentIsInRange()
        {
            Guard.AssertInRange(1, "paramToCheck", 1, 2);
            Guard.AssertInRange(2, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CanAssertArgumentIsBelowRangeBound()
        {
            Guard.AssertInRange(0, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CanAssertArgumentIsGreaterThanUpperRangeBound()
        {
            Guard.AssertInRange(3, "paramToCheck", 1, 2);
        }
    }
}