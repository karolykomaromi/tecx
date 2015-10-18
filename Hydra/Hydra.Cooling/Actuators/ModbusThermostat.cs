namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;
    using Modbus.Device;

    public class ModbusThermostat : Thermostat, IDisposable
    {
        private readonly ModbusSettings settings;

        private readonly IModbusSerialMaster master;

        private readonly Actuator actuator;

        private Celsius targetTemperature;

        public ModbusThermostat(DeviceId id, ModbusSettings settings, IModbusSerialMaster master, Actuator actuator)
            : base(id)
        {
            Contract.Requires(settings != null);
            Contract.Requires(master != null);
            Contract.Requires(actuator != null);

            this.settings = settings;
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
                        this.settings.StartAddress,
                        this.settings.NumberOfRegisters);

                    double temp = ModbusHelper.ConvertTemperatureReadFromRegister(registers[this.actuator]);

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
                    ushort v = ModbusHelper.ConvertTemperatureValueForWriteToRegister((double)value.Value);

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