using System.Diagnostics.Contracts;
using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class PowerAtRpmBuilder : Builder<PowerAtRpm>
    {
        private Power power;
        private ulong rpm;

        public PowerAtRpmBuilder()
        {
            this.power = Measures.Power.Zero;
            this.rpm = 0;
        }

        public PowerAtRpmBuilder Power(Power power)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<PowerAtRpmBuilder>() != null);

            this.power = power;

            return this;
        }

        public PowerAtRpmBuilder AtRpm(ulong rpm)
        {
            Contract.Ensures(Contract.Result<PowerAtRpmBuilder>() != null);

            this.rpm = rpm;

            return this;
        }

        public override PowerAtRpm Build()
        {
            return new PowerAtRpm(this.power, this.rpm);
        }
    }
}