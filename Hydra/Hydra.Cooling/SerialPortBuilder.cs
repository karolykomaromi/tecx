namespace Hydra.Cooling
{
    using System.Diagnostics.Contracts;
    using System.IO.Ports;
    using Hydra.Infrastructure;

    public class SerialPortBuilder : Builder<SerialPort>
    {
        private string portName;

        private BaudRate baudRate;

        private int dataBits;

        private Parity parity;

        private StopBits stopBits;

        public SerialPortBuilder()
        {
            this.portName = "COM1";
            this.baudRate = BaudRate.Bd19200;
            this.dataBits = 8;
            this.parity = Parity.None;
            this.stopBits = StopBits.One;
        }

        public SerialPortBuilder ForPort(string portName)
        {
            Contract.Requires(!string.IsNullOrEmpty(portName));
            Contract.Ensures(Contract.Result<SerialPortBuilder>() != null);

            this.portName = portName;

            return this;
        }

        public SerialPortBuilder WithBaudRate(BaudRate baudRate)
        {
            Contract.Requires(baudRate != null);
            Contract.Ensures(Contract.Result<SerialPortBuilder>() != null);

            this.baudRate = baudRate;

            return this;
        }

        public SerialPortBuilder WithDataBits(int dataBits)
        {
            Contract.Requires(dataBits > 0);
            Contract.Ensures(Contract.Result<SerialPortBuilder>() != null);

            this.dataBits = dataBits;

            return this;
        }

        public SerialPortBuilder WithParity(Parity parity)
        {
            Contract.Ensures(Contract.Result<SerialPortBuilder>() != null);

            this.parity = parity;

            return this;
        }

        public SerialPortBuilder WithStopBits(StopBits stopBits)
        {
            Contract.Ensures(Contract.Result<SerialPortBuilder>() != null);

            this.stopBits = stopBits;

            return this;
        }

        public override SerialPort Build()
        {
            Contract.Ensures(Contract.Result<SerialPort>() != null);

            SerialPort serialPort = new SerialPort(this.portName)
                                    {
                                        BaudRate = this.baudRate,
                                        DataBits = this.dataBits,
                                        Parity = this.parity,
                                        StopBits = this.stopBits
                                    };

            return serialPort;
        }
    }
}