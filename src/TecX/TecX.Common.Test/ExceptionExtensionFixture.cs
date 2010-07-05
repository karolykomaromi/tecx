using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Extensions.Error;

namespace TecX.Common.Test
{
    [TestClass]
    public class ExceptionExtensionFixture
    {
        [TestMethod]
        public void CanAddSingleInfo()
        {
            try
            {
                throw new Exception().WithAdditionalInfo("single info", 42);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(1, ex.Data.Count);
                Assert.AreEqual(42, ex.Data["single info"]);
            }
        }

        [TestMethod]
        public void CanAddMoreInfo()
        {
            try
            {
                throw new Exception().WithAdditionalInfos(new Dictionary<object, object>
                                                              {
                                                                  { "first info", 42 }, 
                                                                  { "scnd info", 666 }
                                                              });
            }
            catch (Exception ex)
            {
                Assert.AreEqual(2, ex.Data.Count);
                Assert.AreEqual(42, ex.Data["first info"]);
                Assert.AreEqual(666, ex.Data["scnd info"]);
            }
        }
    }
}
