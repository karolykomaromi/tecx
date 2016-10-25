namespace Cars.Parts.Engines
{
    using System.Diagnostics.Contracts;
    using Cars.Measures;

    public class TorqueAtRpmBuilder : Builder<TorqueAtRpm>
    {
        private Torque torque;
        private ulong rpm;

        public TorqueAtRpmBuilder()
        {
            this.torque = Measures.Torque.Zero;
            this.rpm = 0;
        }

        public TorqueAtRpmBuilder AtRpm(ulong rpm)
        {
            Contract.Ensures(Contract.Result<TorqueAtRpmBuilder>() != null);

            this.rpm = rpm;

            return this;
        }

        public TorqueAtRpmBuilder Torque(Torque torque)
        {
            Contract.Ensures(Contract.Result<TorqueAtRpmBuilder>() != null);

            this.torque = torque;

            return this;
        }

        public override TorqueAtRpm Build()
        {
            return new TorqueAtRpm(this.torque, this.rpm);
        }
    }
}