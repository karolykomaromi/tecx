namespace Cars.Parts.Engines
{
    using Cars.Measures;

    public class TorqueAtRpm
    {
        private readonly Torque torque;
        private readonly ulong revolutionsPerMinute;

        public TorqueAtRpm(Torque torque, ulong revolutionsPerMinute)
        {
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