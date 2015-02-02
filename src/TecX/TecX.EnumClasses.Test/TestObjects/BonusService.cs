namespace TecX.EnumClasses.Test.TestObjects
{
    public class BonusService
    {
        public void ProcessBonus(Employee employee)
        {
            employee.Bonus = employee.Type.BonusSize;
        }
    }
}