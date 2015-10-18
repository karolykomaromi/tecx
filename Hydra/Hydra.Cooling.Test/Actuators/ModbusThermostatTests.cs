namespace Hydra.Cooling.Test.Actuators
{
    using System.IO.Ports;
    using Hydra.Cooling.Actuators;
    using Modbus.Device;
    using Xunit;

    public class ModbusThermostatTests
    {
        [Fact(Skip = "Won't work without hardware.")]
        public void Should2()
        {
            SerialPort port = new SerialPortBuilder().WithPort(ComPort.Com3);

            port.Open();

            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

            DeviceId deviceId = new DeviceId(41);

            ModbusSettings settings = new ModbusSettings();

            using (var sut = new ModbusThermostat(deviceId, settings, master, Actuator.TargetTemperature))
            {
            }
        }
    }
}