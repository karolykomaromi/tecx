using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Test
{
    [TestClass]
    public class LatchFixture
    {
        [TestMethod]
        public void CanBlockUsingLatch()
        {
            Latch latch = new Latch();

            //the Assert.Fail must never be reached because it is blocked by the latch
            latch.RunOpThatMightRaiseRunawayEvents(() =>
                                                   latch.RunOpProtectedByLatch(
                                                       () => Assert.Fail("must never be reached")));
        }
    }
}