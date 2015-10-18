namespace Hydra.Cooling.Test
{
    using Xunit;

    public class BaudRateTests
    {
        [Fact]
        public void Should_Return_Correct_Int32_Value()
        {
            Assert.Equal(19200, BaudRate.Bd19200);
            Assert.Equal(9600, BaudRate.Bd9600);
        }
    }
}