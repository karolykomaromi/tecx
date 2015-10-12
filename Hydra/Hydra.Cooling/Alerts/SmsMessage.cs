namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Hydra.Cooling.Properties;
    using Hydra.Infrastructure;

    public class SmsMessage : IEquatable<SmsMessage>
    {
        private const uint CompleteMessagePartNumber = 0;

        private const int MaxChunkLength = 160;

        private readonly PhoneNumberCollection recepients;

        private readonly string message;

        private readonly uint partNumber;

        private readonly uint partsTotal;

        public SmsMessage(PhoneNumberCollection recepients, string message)
            : this(recepients, message, SmsMessage.CompleteMessagePartNumber, SmsMessage.CompleteMessagePartNumber)
        {
        }

        private SmsMessage(PhoneNumberCollection recepients, string message, uint partNumber, uint partsTotal)
        {
            Contract.Requires(recepients != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(message));

            this.recepients = recepients;
            this.message = message;
            this.partNumber = partNumber;
            this.partsTotal = partsTotal;
        }

        public PhoneNumberCollection Recepients
        {
            get
            {
                Contract.Ensures(Contract.Result<PhoneNumberCollection>() != null);

                return this.recepients;
            }
        }

        public string Message
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                return this.message;
            }
        }

        public uint PartNumber
        {
            get { return this.partNumber; }
        }

        public uint PartsTotal
        {
            get { return this.partsTotal; }
        }

        public IEnumerable<SmsMessage> Chunkify()
        {
            uint total = (uint)Math.Ceiling((double)this.Message.Length / SmsMessage.MaxChunkLength);

            return StringHelper
                .Chunkify(this.message, SmsMessage.MaxChunkLength)
                .Select((chunk, index) => new SmsMessage(this.Recepients, chunk, (uint)(index + 1), total));
        }

        public bool Equals(SmsMessage other)
        {
            if (other == null)
            {
                return false;
            }

            bool isEqual = this.PartNumber == other.PartNumber &&
                this.Recepients.Equals(other.Recepients) &&
                string.Equals(this.Message, other.Message, StringComparison.Ordinal);

            return isEqual;
        }

        public override bool Equals(object obj)
        {
            SmsMessage other = obj as SmsMessage;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = this.PartNumber.GetHashCode() ^ 
                this.Message.GetHashCode() ^ 
                this.Recepients.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return string.Format(Resources.TextMessage_Format_String, this.Recepients, this.Message);
        }
    }
}