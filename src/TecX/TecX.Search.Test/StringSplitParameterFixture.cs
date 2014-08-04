namespace TecX.Search.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Split;

    [TestClass]
    public class StringSplitParameterFixture
    {
        [TestMethod]
        public void CanComposeParameters()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");
            StringSplitParameter p2 = new StringSplitParameter("2");

            CompositeStringSplitParameter composite = new CompositeStringSplitParameter();

            composite.Add(p1);
            composite.Add(p2);

            Assert.AreEqual("1 2", composite.Value);
        }

        [TestMethod]
        public void ToStringReturnValue()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");

            Assert.AreEqual(p1.ToString(), p1.Value);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotSetValueToNull()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");

            p1.Value = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSetValueToEmptySTring()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");

            p1.Value = string.Empty;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotSetValueOfCompositeStringSplitParameter()
        {
            CompositeStringSplitParameter p1 = new CompositeStringSplitParameter();

            p1.Value = "1";
        }

        [TestMethod]
        public void ComparisonWithNullReturnsFalse()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");

            Assert.IsFalse(p1.Equals(null as StringSplitParameter));
        }
    }
}