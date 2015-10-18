namespace Hydra.Cooling.Test
{
    using Xunit;

    public class ComPortTests
    {
        [Fact]
        public void Should_UpperCase_Name()
        {
            Assert.Equal("COM1", ComPort.Com1);
            Assert.Equal("COM2", ComPort.Com2);
            Assert.Equal("COM3", ComPort.Com3);
        }

        [Fact]
        public void Should_Include_All_Well_Known_Ports()
        {
            Assert.Equal(9, ComPort.WellKnownPorts.Count);
        }
    }
}