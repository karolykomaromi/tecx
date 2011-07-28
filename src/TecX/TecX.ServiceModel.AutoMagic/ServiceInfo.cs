using System.ServiceModel;
using System.ServiceModel.Channels;

using TecX.Common;

namespace TecX.ServiceModel.AutoMagic
{
    public class ServiceInfo
    {
        private readonly EndpointAddress _address;
        private readonly Binding _binding;

        public EndpointAddress Address
        {
            get
            {
                return this._address;
            }
        }

        public Binding Binding
        {
            get
            {
                return this._binding;
            }
        }

        public ServiceInfo(EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            this._address = address;
            _binding = binding;
        }
    }
}