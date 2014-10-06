namespace TecX.EnumClasses.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Test.TestObjects;

    [TestClass]
    public class EnumerationFixture
    {
        [TestMethod]
        public void Should_SetBonus_To_EmployeeType_BonusSize()
        {
            var employee = new Employee { Type = EmployeeType.Manager };

            var svc = new BonusService();

            svc.ProcessBonus(employee);

            Assert.AreEqual(2000m, employee.Bonus);
        }
    }
}
