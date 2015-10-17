namespace Hydra.Cooling.Test.Actuators
{
    using System.IO.Ports;
    using Hydra.Cooling.Actuators;
    using Modbus.Device;
    using Xunit;

    public class ModbusThermostatTests
    {
        [Fact]
        public void Should2()
        {
            SerialPort port = new SerialPortBuilder().ForPort("COM3");

            port.Open();

            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

            DeviceId deviceId = new DeviceId(41);

            using (var sut = new ModbusThermostat(deviceId, master, Actuator.TargetTemperature))
            {
            }
        }
    }
}