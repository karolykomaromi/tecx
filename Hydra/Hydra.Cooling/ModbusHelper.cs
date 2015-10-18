namespace Hydra.Cooling
{
    public static class ModbusHelper
    {
        public static double ConvertTemperatureReadFromRegister(ushort value)
        {
            double converted;

            if (value > short.MaxValue)
            {
                converted = (int)value - ushort.MaxValue - 1;
            }
            else
            {
                converted = value;
            }

            return converted / 10;
        }

        public static ushort ConvertTemperatureValueForWriteToRegister(double value)
        {
            ushort converted;

            value = value * 10;

            if (value <= 0)
            {
                converted = (ushort)(value + ushort.MaxValue + 1);
            }
            else
            {
                converted = (ushort)value;
            }

            return converted;
        }
    }
}