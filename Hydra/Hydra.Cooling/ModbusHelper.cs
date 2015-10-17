namespace Hydra.Cooling
{
    public static class ModbusHelper
    {
        public const ushort StartAddress = 0;

        public const ushort NumRegisters = 38;

        public static short ConvertRegisterValueToOutput(ushort value)
        {
            short converted;

            if (value > short.MaxValue)
            {
                converted = (short)((int)value - ushort.MaxValue - 1);
            }
            else
            {
                converted = (short)value;
            }

            return converted;
        }

        public static ushort ConvertRegisterValueToInput(double value)
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