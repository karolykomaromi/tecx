namespace Hydra.Cooling.Alerts
{
    using System.Collections.Generic;
    using System.Linq;

    public class SmsTransmissionProtocol
    {
        public static readonly SmsTransmissionProtocol Empty = new SmsTransmissionProtocol();

        private readonly IReadOnlyCollection<TransmissionSucceeded> succeeded;
        private readonly IReadOnlyCollection<TransmissionFailed> failed;

        public SmsTransmissionProtocol(params ProtocolItem[] details)
        {
            details = details ?? new ProtocolItem[0];

            this.succeeded = details.OfType<TransmissionSucceeded>().ToArray();
            this.failed = details.OfType<TransmissionFailed>().ToArray();
        }

        private SmsTransmissionProtocol()
        {
            this.succeeded = new TransmissionSucceeded[0];
            this.failed = new TransmissionFailed[0];
        }

        public bool WasSuccessfull
        {
            get { return this.Succeeded.Count > 0 && this.Failed.Count == 0;  }
        }

        public bool WasPartiallySuccessfull
        {
            get { return this.Succeeded.Count > 0 && this.Failed.Count > 0; }
        }

        public bool WasFailure
        {
            get { return this.Succeeded.Count == 0 && this.Failed.Count > 0; }
        }

        public IReadOnlyCollection<TransmissionSucceeded> Succeeded
        {
            get { return this.succeeded; }
        }

        public IReadOnlyCollection<TransmissionFailed> Failed
        {
            get { return this.failed; }
        }
    }
}