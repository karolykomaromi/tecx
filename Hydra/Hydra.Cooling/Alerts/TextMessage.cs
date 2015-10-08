namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Hydra.Cooling.Properties;
    using Hydra.Infrastructure;

    public class TextMessage : IEquatable<TextMessage>
    {
        private const uint CompleteMessagePartNumber = 0;

        private const int MaxChunkLength = 160;

        private readonly PhoneNumber recepient;

        private readonly string message;

        private readonly uint partNumber;

        private readonly uint partsTotal;

        public TextMessage(PhoneNumber recepient, string message)
            : this(recepient, message, TextMessage.CompleteMessagePartNumber, TextMessage.CompleteMessagePartNumber)
        {
        }

        private TextMessage(PhoneNumber recepient, string message, uint partNumber, uint partsTotal)
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

        public IEnumerable<TextMessage> Chunkify()
        {
            uint total = (uint)Math.Ceiling((double)this.Message.Length / TextMessage.MaxChunkLength);

            return StringHelper
                .Chunkify(this.message, TextMessage.MaxChunkLength)
                .Select((chunk, index) => new TextMessage(this.Recepient, chunk, (uint)(index + 1), total));
        }

        public bool Equals(TextMessage other)
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
            TextMessage other = obj as TextMessage;

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