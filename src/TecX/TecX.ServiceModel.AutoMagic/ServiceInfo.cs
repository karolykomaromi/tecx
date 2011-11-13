namespace TecX.ServiceModel.AutoMagic
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using TecX.Common;

    public class ServiceInfo
    {
        private readonly EndpointAddress address;
        private readonly Binding binding;

        public ServiceInfo(EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            this.address = address;
            this.binding = binding;
        }

        public EndpointAddress Address
        {
            get
            {
                return this.address;
            }
        }

        public Binding Binding
        {
            get
            {
                return this.binding;
            }
        }
    }
}