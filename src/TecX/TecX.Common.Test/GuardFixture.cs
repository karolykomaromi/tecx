using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Test
{
    /// <summary>
    /// Summary description for TestArgumentHelper
    /// </summary>
    [TestClass]
    public class GuardFixture
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void CanAssertArgumentIsInRange()
        {
            Guard.AssertIsInRange(1, "paramToCheck", 1, 2);
            Guard.AssertIsInRange(2, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void CanAssertArgumentIsBelowRangeBound()
        {
            Guard.AssertIsInRange(0, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void CanAssertArgumentIsGreaterThanUpperRangeBound()
        {
            Guard.AssertIsInRange(3, "paramToCheck", 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void CanAssertArgumentIsOfWrongType()
        {
            Guard.AssertIsType<ClassA>(string.Empty, "paramToCheck");
        }

        [TestMethod]
        public void CanAssertArgumentIsOfCorrectType()
        {
            Guard.AssertIsType<ClassA>(new ClassB(), "paramToCheck");
        }
    }

    internal class ClassA
    {
    }

    internal class ClassB : ClassA
    {
    }
}