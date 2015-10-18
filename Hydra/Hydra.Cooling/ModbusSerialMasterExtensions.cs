namespace Hydra.Cooling
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Modbus.Device;

    public static class ModbusSerialMasterExtensions
    {
        public static Task<IReadOnlyCollection<byte>> ScanForDevicesAsync(this IModbusSerialMaster master)
        {
            Contract.Requires(master != null);

            return Task<IReadOnlyCollection<byte>>.Factory.StartNew(
                () =>
                {
                    IModbusSerialMaster copy = master;

                    var validIds = new List<byte>();

                    for (byte id = 0; id < byte.MaxValue; id++)
                    {
                        try
                        {
                            if (copy.ReturnQueryData(id, ushort.MaxValue))
                            {
                                validIds.Add(id);
                            }
                        }
                        catch
                        {
                            // weberse 2015-10-18 An Exception of any kind means we couldn't reach the device.
                        }
                    }

                    return validIds;
                });
        }
    }
}