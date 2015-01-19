namespace Hydra.Import
{
    using System.Diagnostics.Contracts;

    public abstract class ImportMessage
    {
        public static readonly ImportMessage Empty = new EmptyMessage();

        private readonly string message;
        private Location location;

        protected ImportMessage(string message)
        {
            this.message = message;
            this.Location = Location.Nowhere;
        }

        public Location Location
        {
            get
            {
                Contract.Ensures(Contract.Result<Location>() != null);

                return this.location;
            }

            set
            {
                Contract.Requires(value != null);

                this.location = value;
            }
        }

        public override string ToString()
        {
            return this.Location + ": " + this.message;
        }

        private class EmptyMessage : ImportMessage
        {
            public EmptyMessage()
                : base(string.Empty)
            {
            }
        }
    }
}