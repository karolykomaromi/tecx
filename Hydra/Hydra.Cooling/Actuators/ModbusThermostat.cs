namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;
    using Modbus.Device;

    public class ModbusThermostat : Thermostat, IDisposable
    {
        private readonly IModbusSerialMaster master;

        private readonly Actuator actuator;

        private Celsius targetTemperature;

        public ModbusThermostat(DeviceId id, IModbusSerialMaster master, Actuator actuator)
            : base(id)
        {
            Contract.Requires(master != null);
            Contract.Requires(actuator != null);

            this.master = master;
            this.actuator = actuator;
        }

        public override Temperature TargetTemperature
        {
            get
            {
                if (this.targetTemperature == null)
                {
                    ushort[] registers = this.master.ReadHoldingRegisters(
                        this.Id,
                        ModbusHelper.StartAddress,
                        ModbusHelper.NumRegisters);

                    double temp = (double)ModbusHelper.ConvertRegisterValueToOutput(registers[this.actuator]) / 10;

                    this.targetTemperature = temp.DegreesCelsius();
                }

                return this.targetTemperature;
            }

            set
            {
                Celsius oldTargetTemperature = this.targetTemperature;
                Celsius newTargetTemperature = value.ToCelsius();

                if (oldTargetTemperature != newTargetTemperature)
                {
                    ushort v = ModbusHelper.ConvertRegisterValueToInput((double)value.Value);

                    this.master.WriteSingleRegister(this.Id, this.actuator, v);

                    this.targetTemperature = newTargetTemperature;

                    this.OnTargetTemperatureChanged(oldTargetTemperature, newTargetTemperature);
                }
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