namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using Modbus.Device;

    public class ModbusTemperatureSensor : TemperatureSensor, IDisposable
    {
        private readonly ModbusSettings settings;

        private readonly IModbusSerialMaster master;

        private readonly Probe probe;

        public ModbusTemperatureSensor(DeviceId deviceId, ModbusSettings settings, IModbusSerialMaster master, Probe probe)
            : base(deviceId)
        {
            Contract.Requires(master != null);
            Contract.Requires(settings != null);
            Contract.Requires(probe != null);

            this.settings = settings;
            this.master = master;
            this.probe = probe;
        }

        public override Temperature CurrentTemperature
        {
            get
            {
                ushort[] registers = this.master.ReadHoldingRegisters(this.Id, this.settings.StartAddress, this.settings.NumberOfRegisters);

                double temp = ModbusHelper.ConvertTemperatureReadFromRegister(registers[this.probe]);

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