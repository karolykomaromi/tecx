using System.Diagnostics.Contracts;
using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class PowerAtRpm
    {
        private readonly Power power;
        private readonly ulong revolutionsPerMinute;

        public PowerAtRpm(Power power, ulong revolutionsPerMinute)
        {
            Contract.Requires(power != null);

            this.power = power;
            this.revolutionsPerMinute = revolutionsPerMinute;
        }

        public Power Power
        {
            get { return this.power; }
        }

        public ulong RevolutionsPerMinute
        {
            get { return this.revolutionsPerMinute; }
        }
    }
}