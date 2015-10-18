namespace Hydra.Cooling.Test
{
    using Xunit;
    using Xunit.Extensions;

    public class ModbusHelperTests
    {
        [Theory]
        [InlineData((ushort)206, 20.6)]
        [InlineData((ushort)306, 30.6)]
        [InlineData((ushort)65236, -30)]
        [InlineData((ushort)65331, -20.5)]
        public void Should_Convert_Temperature_Read_From_Device(ushort read, double expected)
        {
            Assert.Equal(expected, ModbusHelper.ConvertTemperatureReadFromRegister(read));
        }

        [Theory]
        [InlineData(20.6, (ushort)206)]
        [InlineData(30.6, (ushort)306)]
        [InlineData(-30, (ushort)65236)]
        [InlineData(-20.5, (ushort)65331)]
        public void Should_Convert_Temperature_To_Write_Device(double toWrite, ushort expected)
        {
            Assert.Equal(expected, ModbusHelper.ConvertTemperatureValueForWriteToRegister(toWrite));
        }
    }
}