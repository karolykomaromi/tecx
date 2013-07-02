namespace TecX.BehavioralTesting.TestObjects
{
    public class DefaultTaxCalculator : ITaxCalculator
    {
        public double Calculate(Customer customer)
        {
            return 10.0;
        }
    }
}