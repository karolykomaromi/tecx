using System.Diagnostics.Contracts;

namespace Cars.Parts.Engines
{
    public class TransmissionBuilder : Builder<Transmission>
    {
        private TransmissionType type;
        private byte gears;

        public TransmissionBuilder()
        {
            this.type = TransmissionTypes.None;
            this.gears = 0;
        }

        public TransmissionBuilder Type(TransmissionType type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<TransmissionBuilder>() != null);

            this.type = type;

            return this;
        }

        public TransmissionBuilder NumberOfGears(byte gears)
        {
            Contract.Ensures(Contract.Result<TransmissionBuilder>() != null);

            this.gears = gears;

            return this;
        }

        public override Transmission Build()
        {
            return new Transmission
            {
                TransmissionType = this.type,
                NumberOfGears = this.gears
            };
        }
    }
}