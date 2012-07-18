namespace TecX.Search.Test
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Split;
    using TecX.Search.Test.TestObjects;

    [TestClass]
    public class StringSplitStrategyFixture
    {
        [TestMethod]
        public void CanSplitByDate()
        {
            string pattern = "2011-11-14";

            var strategy = new DateTimeStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual(string.Empty, context.StringToSplit);
            Assert.AreEqual(1, context.SplitResults.Count);
            Assert.AreEqual("2011-11-14", context.SplitResults.Single().Value);
        }

        [TestMethod]
        public void CanSplitByDateAndTime()
        {
            string pattern = "2011-11-14 17:31";

            var strategy = new DateTimeStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual(string.Empty, context.StringToSplit);
            Assert.AreEqual(1, context.SplitResults.Count);
            Assert.AreEqual("2011-11-14 17:31", context.SplitResults.Single().Value);
        }

        [TestMethod]
        public void CanSplitByWhitespaces()
        {
            string pattern = "1 2 abc";

            var strategy = new WhitespaceStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual(string.Empty, context.StringToSplit);
            Assert.AreEqual("1", context.SplitResults.ElementAt(0).Value);
            Assert.AreEqual("2", context.SplitResults.ElementAt(1).Value);
            Assert.AreEqual("abc", context.SplitResults.ElementAt(2).Value);
        }

        [TestMethod]
        public void DoesNotReuseDateTimeParts()
        {
            string pattern = "2011-1-1 11:00 2011-6-1 13:00";

            var strategy = new DateTimeStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual(string.Empty, context.StringToSplit);
            Assert.AreEqual(2, context.SplitResults.Count);
            Assert.AreEqual("2011-1-1 11:00", context.SplitResults.ElementAt(0).Value);
            Assert.AreEqual("2011-6-1 13:00", context.SplitResults.ElementAt(1).Value);
        }

        [TestMethod]
        public void CanSplitUnorderedDateTimeParameters()
        {
            string pattern = "2011-11-8 aa 2011-11-8  23:59";

            var strategy = new DateTimeStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual(2, context.SplitResults.Count);
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("2011-11-8")));
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("2011-11-8 23:59")));
        }

        [TestMethod]
        public void CanSplitByNumbers()
        {
            string pattern = "908$§ ali %99";

            var strategy = new NumericalStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual("$§ ali %", context.StringToSplit);
            Assert.AreEqual(2, context.SplitResults.Count);
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("908")));
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("99")));
        }

        [TestMethod]
        public void CanSplitBySpecialChars()
        {
            string pattern = "908$§ ali %99";

            var strategy = new SpecialCharStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual("908 ali 99", context.StringToSplit);
            Assert.AreEqual(2, context.SplitResults.Count);
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("$§")));
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("%")));
        }

        [TestMethod]
        public void CanSplitByAlphabeticalChars()
        {
            string pattern = "908$§ ali %99";

            var strategy = new AlphabeticalStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual("908$§  %99", context.StringToSplit);
            Assert.AreEqual(1, context.SplitResults.Count);
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("ali")));
        }

        [TestMethod]
        public void CanRunSplitChain()
        {
            string pattern = "908$§ ali %99";

            var chain = new StringSplitStrategyChain();

            var strategy = new NullStringSplitStrategy();

            chain.AddStrategy(strategy);

            chain.Split(pattern);

            Assert.IsNotNull(chain.Result);
            Assert.AreEqual(1, chain.Result.SplitResults.Count);
            Assert.AreEqual(pattern, chain.Result.SplitResults.First().Value);
            Assert.AreEqual(string.Empty, chain.Result.StringToSplit);
        }

        [TestMethod]
        public void CanSplitByIpAddress()
        {
            string pattern = "192.168. 192.168.2.1 192.168.0";
            
            var strategy = new IpAddressStringSplitStrategy();

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.AreEqual("192.168.  192.168.0", context.StringToSplit);
            Assert.AreEqual(1, context.SplitResults.Count);
            Assert.AreEqual("192.168.2.1", context.SplitResults[0].Value);
        }
    }
}
