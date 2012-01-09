namespace TecX.Common.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Error;
    using TecX.Common.Test.TestObjects;

    [TestClass]
    public class GuardFixture
    {
        [TestMethod]
        public void CanAssertArgumentIsInRange()
        {
            Guard.AssertIsInRange(1, "paramToCheck", 1, 2);
            Guard.AssertIsInRange(2, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (GuardArgumentOutOfRangeException))]
        public void CanAssertArgumentIsBelowRangeBound()
        {
            Guard.AssertIsInRange(0, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (GuardArgumentOutOfRangeException))]
        public void CanAssertArgumentIsGreaterThanUpperRangeBound()
        {
            Guard.AssertIsInRange(3, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (GuardArgumentOutOfRangeException))]
        public void CanAssertArgumentIsOfWrongType()
        {
            Guard.AssertIsType(typeof(ClassA), string.Empty, "paramToCheck");
        }

        [TestMethod]
        public void CanAssertArgumentIsOfCorrectType()
        {
            Guard.AssertIsType(typeof(ClassA), new ClassB(), "paramToCheck");
        }
    }
}