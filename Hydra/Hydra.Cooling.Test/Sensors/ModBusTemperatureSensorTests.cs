namespace Hydra.Cooling.Test.Sensors
{
    using System.IO.Ports;
    using Hydra.Cooling.Sensors;
    using Modbus.Device;
    using Xunit;

    public class ModbusTemperatureSensorTests
    {
        [Fact(Skip = "Won't work without hardware.")]
        public void Should()
        {
            SerialPort port = new SerialPortBuilder().WithPort(ComPort.Com3);

            port.Open();

            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

            DeviceId deviceId = new DeviceId(41);

            ModbusSettings settings = new ModbusSettings();

            using (var sut = new ModbusTemperatureSensor(deviceId, settings, master, Probe.Room))
            {
            }
        }
    }
}
