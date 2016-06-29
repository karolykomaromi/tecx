using System.Diagnostics.Contracts;
using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class TorqueAtRpm
    {
        private readonly Torque torque;
        private readonly ulong revolutionsPerMinute;

        public TorqueAtRpm(Torque torque, ulong revolutionsPerMinute)
        {
            Contract.Requires(torque != null);

            this.torque = torque;
            this.revolutionsPerMinute = revolutionsPerMinute;
        }

        public Torque Torque
        {
            get { return this.torque; }
        }

        public ulong RevolutionsPerMinute
        {
            get { return this.revolutionsPerMinute; }
        }
    }
}