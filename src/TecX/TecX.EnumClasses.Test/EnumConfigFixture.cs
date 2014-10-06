namespace TecX.EnumClasses.Test
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Test.TestObjects;

    [TestClass]
    public class EnumConfigFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            MyConfigSection section = (MyConfigSection)ConfigurationManager.GetSection("mySection");

            Assert.IsNotNull(section);
            Assert.AreEqual(Numbers.Four, section.MyEnum);
        }
    }
}
