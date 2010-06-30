using System;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Test
{
    /// <summary>
    /// Summary description for RegexBuilderFixture
    /// </summary>
    [TestClass]
    public class RegexBuilderFixture
    {
        public RegexBuilderFixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        public void CanBuildRegexForSingleChar()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar()
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("A"));
            Assert.IsTrue(expression.IsMatch("9"));
            Assert.IsTrue(expression.IsMatch("$"));

            Assert.IsFalse(expression.IsMatch("aa"));
        }

        [TestMethod]
        public void CanBuildRegexForAnyNumberOfChars()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar().OccursZeroOrMoreTimes()
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("aB"));
            Assert.IsTrue(expression.IsMatch("z&"));
            Assert.IsTrue(expression.IsMatch("6b"));
            Assert.IsTrue(expression.IsMatch(string.Empty));
        }

        [TestMethod]
        public void CanBuildRegexForAtLeastOneOccurenceOfAnySingleChar()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar()
                .OccursAtLeastOnce()
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("asdf"));
            Assert.IsTrue(expression.IsMatch("a"));
            Assert.IsFalse(expression.IsMatch(string.Empty));
        }

        [TestMethod]
        public void CanBuildRegexForAtMostCertainNumberOfOccurencesOfAnySingleChar()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar()
                .OccursAtMost(3)
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch(string.Empty));
            Assert.IsTrue(expression.IsMatch("a"));
            Assert.IsTrue(expression.IsMatch("ab"));
            Assert.IsTrue(expression.IsMatch("abc"));
            Assert.IsFalse(expression.IsMatch("abcd"));
        }

        [TestMethod]
        public void CanBuildRegexForCertainNumberOfOccurencesOfAnySingleChar()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar()
                .OccursForSpecificNumberOfTimes(3)
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("asd"));
            Assert.IsFalse(expression.IsMatch("af"));
            Assert.IsFalse(expression.IsMatch("asdf"));
        }

        [TestMethod]
        public void CanBuildRegexForSpecificRangeOfOccurencesOfAnySingleChar()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AnyChar()
                .OccursForRangeOfNumberOfTimes(3, 4)
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("asd"));
            Assert.IsTrue(expression.IsMatch("asdf"));
            Assert.IsFalse(expression.IsMatch("as"));
            Assert.IsFalse(expression.IsMatch("asdfg"));
        }

        [TestMethod]
        public void CanBuildRegexForSingleHexDigit()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AHexDigit()
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("0"));
            Assert.IsTrue(expression.IsMatch("9"));
            Assert.IsTrue(expression.IsMatch("a")); 
            Assert.IsTrue(expression.IsMatch("A")); 
            Assert.IsTrue(expression.IsMatch("f"));
            Assert.IsTrue(expression.IsMatch("F"));

            Assert.IsFalse(expression.IsMatch("z"));
        }

        [TestMethod]
        public void CanBuildRegexForGuid()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .AHexDigit()
                .OccursForSpecificNumberOfTimes(8)
                .ASpecificChar('-')
                .AHexDigit()
                .OccursForSpecificNumberOfTimes(4)
                .ASpecificChar('-')
                .AHexDigit()
                .OccursForSpecificNumberOfTimes(4)
                .ASpecificChar('-')
                .AHexDigit()
                .OccursForSpecificNumberOfTimes(4)
                .ASpecificChar('-')
                .AHexDigit()
                .OccursForSpecificNumberOfTimes(12)
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch(Guid.NewGuid().ToString()));
        }

        [TestMethod]
        public void CanBuildRegexWithSpecificChars()
        {
            Regex expression = new RegexBuilder()
                .StartingFromBeginning()
                .ASpecificChar('-')
                .ToEndOfString();

            Assert.IsTrue(expression.IsMatch("-"));
            Assert.IsFalse(expression.IsMatch("a-a"));
        }
    }
}
