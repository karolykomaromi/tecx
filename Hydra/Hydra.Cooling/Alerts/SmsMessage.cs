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

        private readonly PhoneNumber recepient;

        private readonly string message;

        private readonly uint partNumber;

        private readonly uint partsTotal;

        public SmsMessage(PhoneNumber recepient, string message)
            : this(recepient, message, SmsMessage.CompleteMessagePartNumber, SmsMessage.CompleteMessagePartNumber)
        {
        }

        private SmsMessage(PhoneNumber recepient, string message, uint partNumber, uint partsTotal)
        {
            Contract.Requires(recepient != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(message));

            this.recepient = recepient;
            this.message = message;
            this.partNumber = partNumber;
            this.partsTotal = partsTotal;
        }

        public PhoneNumber Recepient
        {
            get
            {
                Contract.Ensures(Contract.Result<PhoneNumber>() != null);

                return this.recepient;
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
                .Select((chunk, index) => new SmsMessage(this.Recepient, chunk, (uint)(index + 1), total));
        }

        public bool Equals(SmsMessage other)
        {
            if (other == null)
            {
                return false;
            }

            bool isEqual = this.PartNumber == other.PartNumber &&
                this.Recepient == other.Recepient &&
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
                this.Recepient.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return string.Format(Resources.TextMessage_Format_String, this.Recepient, this.Message);
        }
    }
}