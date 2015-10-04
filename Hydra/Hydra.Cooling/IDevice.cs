namespace Hydra.Cooling
{
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.I18n;

    [ContractClass(typeof(DeviceContract))]
    public interface IDevice
    {
        DeviceId Id { get; }

        PolyglotString Name { get; set; }

        PolyglotString Description { get; set; }

        Location Location { get; set; }
    }

    [ContractClassFor(typeof(IDevice))]
    internal abstract class DeviceContract : IDevice
    {
        public DeviceId Id
        {
            get
            {
                Contract.Ensures(Contract.Result<DeviceId>() != null);

                return DeviceId.Empty;
            }
        }

        public PolyglotString Name
        {
            get
            {
                Contract.Ensures(Contract.Result<PolyglotString>() != null);
                return PolyglotString.Empty;
            }

            set
            {
                Contract.Requires(value != null);
            }
        }

        public PolyglotString Description
        {
            get
            {
                Contract.Ensures(Contract.Result<PolyglotString>() != null);
                return PolyglotString.Empty;
            }

            set
            {
                Contract.Requires(value != null);
            }
        }

        public Location Location
        {
            get
            {
                Contract.Ensures(Contract.Result<Location>() != null);
                return Location.Nowhere;
            }

            set
            {
                Contract.Requires(value != null);
            }
        }
    }
}