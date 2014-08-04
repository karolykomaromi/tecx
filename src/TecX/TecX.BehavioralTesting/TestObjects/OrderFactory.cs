namespace TecX.BehavioralTesting.TestObjects
{
    public class OrderFactory
    {
        private readonly ITaxCalculator calculator;

        public OrderFactory(ITaxCalculator calculator)
        {
            this.calculator = calculator;
        }

        public Order Build(Customer customer)
        {
            return new Order { Tax = calculator.Calculate(customer) };
        }
    }
}