namespace TecX.Search.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;
    using TecX.Search.Parse;
    using TecX.Search.Split;

    [TestClass]
    public class ParameterParseStrategyFixture
    {
        [TestMethod]
        public void CanParseInt32()
        {
            string pattern = "123";

            var strategy = new Int32ParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<int>));
            Assert.AreEqual(123, context.Parameter.Value);
        }

        [TestMethod]
        public void CanParseInt64()
        {
            string pattern = "123";

            var strategy = new Int64ParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<long>));
            Assert.AreEqual(123L, context.Parameter.Value);
        }

        [TestMethod]
        public void CanParseDateTime()
        {
            string pattern = "2011-11-14";

            var strategy = new DateTimeParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<DateTime>));
            Assert.AreEqual(new DateTime(2011, 11, 14), context.Parameter.Value);
        }

        [TestMethod]
        public void CanParseTimePortionOnly()
        {
            string pattern = "12:51:37.019";

            var strategy = new DateTimeParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            DateTime expectedParseResult = DateTime.Today.AddHours(12).AddMinutes(51).AddSeconds(37).AddMilliseconds(19);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<DateTime>));
            Assert.AreEqual(expectedParseResult, context.Parameter.Value);
        }

        [TestMethod]
        public void CanParseFloat()
        {
            string pattern = 1.23.ToString("F2", Defaults.Culture);

            var strategy = new FloatParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<float>));
            Assert.AreEqual(1.23f, context.Parameter.Value);
        }

        [TestMethod]
        public void CanParseDouble()
        {
            string pattern = 1.23.ToString("F2", Defaults.Culture);

            var strategy = new DoubleParseStrategy();

            var context = new ParameterParseContext(new StringSplitParameter(pattern));

            strategy.Parse(context);

            Assert.IsTrue(context.ParseComplete);
            Assert.AreNotEqual(SearchParameter.Empty, context.Parameter);
            Assert.IsInstanceOfType(context.Parameter, typeof(SearchParameter<double>));
            Assert.AreEqual(1.23, context.Parameter.Value);
        }
    }
}
