namespace Hydra.Cooling.Alerts
{
    public abstract class ProtocolItem
    {
        private readonly PhoneNumber recepient;

        protected ProtocolItem(PhoneNumber recepient)
        {
            this.recepient = recepient;
        }

        public PhoneNumber Recepient
        {
            get { return this.recepient; }
        }
    }
}