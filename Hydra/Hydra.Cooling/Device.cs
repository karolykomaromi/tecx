namespace Hydra.Cooling
{
    using Hydra.Infrastructure.I18n;

    public abstract class Device : IDevice
    {
        public static readonly IDevice Null = new NullDevice();

        protected Device()
        {
            this.Name = PolyglotString.Empty;
            this.Description = PolyglotString.Empty;
            this.Location = Location.Nowhere;
        }

        public PolyglotString Name { get; set; }

        public PolyglotString Description { get; set; }

        public Location Location { get; set; }

        protected class NullDevice : IDevice
        {
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