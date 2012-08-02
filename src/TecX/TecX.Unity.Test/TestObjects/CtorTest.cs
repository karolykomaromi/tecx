namespace TecX.Unity.Test.TestObjects
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class CtorTest : ICtorTest
    {
        public CtorTest(ILogger logger)
        {
            Assert.Fail("must not be called");
        }

        public CtorTest(string connectionString, ILogger logger)
        {
            Assert.AreEqual("blablub", connectionString);
            Assert.IsNotNull(logger);
        }

        public CtorTest(string connectionString, ILogger logger, int anotherParam)
        {
            Assert.Fail("must not be selected");
        }

        public CtorTest(string connectionString, ILogger logger, string anotherParam)
        {
            Assert.AreEqual("blablub", connectionString);
            Assert.IsNotNull(logger);
            Assert.AreEqual("I'm a string", anotherParam);
        }
    }
}