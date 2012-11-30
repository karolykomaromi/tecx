namespace TecX.Common.Test.Extensions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Extensions.Error;

    [TestClass]
    public class ExceptionExtensionsFixture
    {
        [TestMethod]
        public void CanAddSingleInfo()
        {
            Exception exception = new Exception().WithAdditionalInfo("1", "2");

            Assert.AreEqual("2", exception.Data["1"]);
        }

        [TestMethod]
        public void CanAddMultipleInfos()
        {
            Exception exception = new Exception().WithAdditionalInfos(new Dictionary<object, object>
                {
                    { "1", "2" }, 
                    { "3", "4" }
                });

            Assert.AreEqual("2", exception.Data["1"]);
            Assert.AreEqual("4", exception.Data["3"]);
        }
    }
}
