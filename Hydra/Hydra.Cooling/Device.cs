namespace Hydra.Cooling
{
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.I18n;

    public abstract class Device : IDevice
    {
        public static readonly IDevice Null = new NullDevice();

        private readonly DeviceId id;

        protected Device(DeviceId id)
        {
            Contract.Requires(id != null);

            this.id = id;
            this.Name = PolyglotString.Empty;
            this.Description = PolyglotString.Empty;
            this.Location = Location.Nowhere;
        }

        public PolyglotString Name { get; set; }

        public PolyglotString Description { get; set; }

        public Location Location { get; set; }

        public DeviceId Id
        {
            get { return this.id; }
        }

        protected class NullDevice : IDevice
        {
            public DeviceId Id
            {
                get { return DeviceId.Empty; }
            }

            public PolyglotString Name
            {
                get { return PolyglotString.Empty; }
                set { }
            }

            public PolyglotString Description
            {
                get { return PolyglotString.Empty; }
                set { }
            }

            public Location Location
            {
                get { return Location.Nowhere; }
                set { }
            }
        }
    }
}