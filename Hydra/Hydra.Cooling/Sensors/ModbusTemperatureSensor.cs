namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using Modbus.Device;

    public class ModbusTemperatureSensor : TemperatureSensor, IDisposable
    {
        private readonly IModbusSerialMaster master;

        private readonly Probe probe;

        public ModbusTemperatureSensor(DeviceId deviceId, IModbusSerialMaster master, Probe probe)
            : base(deviceId)
        {
            Contract.Requires(master != null);
            Contract.Requires(probe != null);

            this.master = master;
            this.probe = probe;
        }

        public override Temperature CurrentTemperature
        {
            get
            {
                ushort[] registers = this.master.ReadHoldingRegisters(this.Id, ModbusHelper.StartAddress, ModbusHelper.NumRegisters);

                double temp = (double)ModbusHelper.ConvertRegisterValueToOutput(registers[this.probe]) / 10;

                return temp.DegreesCelsius();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.master.Dispose();
            }
        }
    }
}