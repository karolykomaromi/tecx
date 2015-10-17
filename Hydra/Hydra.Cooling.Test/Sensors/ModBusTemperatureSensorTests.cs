namespace Hydra.Cooling.Test.Sensors
{
    using System.IO.Ports;
    using Hydra.Cooling.Sensors;
    using Modbus.Device;
    using Xunit;

    public class ModbusTemperatureSensorTests
    {
        [Fact]
        public void Should()
        {
            SerialPort port = new SerialPortBuilder().ForPort("COM3");

            port.Open();

            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

            DeviceId deviceId = new DeviceId(41);

            using (var sut = new ModbusTemperatureSensor(deviceId, master, Probe.Room))
            {
            }
        }
    }
}
