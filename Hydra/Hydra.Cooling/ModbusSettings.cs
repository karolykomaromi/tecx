namespace Hydra.Cooling
{
    public class ModbusSettings
    {
        public ModbusSettings()
        {
            this.StartAddress = 0;
            this.NumberOfRegisters = 38;
        }

        public virtual ushort StartAddress { get; set; }

        public virtual ushort NumberOfRegisters { get; set; }
    }
}