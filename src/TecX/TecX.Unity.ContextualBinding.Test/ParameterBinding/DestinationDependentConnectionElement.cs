namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    public class DestinationDependentConnectionElement : InjectionMemberElement
    {
        private static class Constants
        {
            public const string EndpointAddress = "endpointAddress";

            public const string ParameterName = "parameterName";

            public const string ConnectionStringOrName = "connectionStringOrName";
        }

        [ConfigurationProperty(Constants.EndpointAddress, IsRequired = true)]
        public string EndpointAddress
        {
            get
            {
                return (string)this[Constants.EndpointAddress];
            }

            set
            {
                this[Constants.EndpointAddress] = (object)value;
            }
        }

        [ConfigurationProperty(Constants.ParameterName, IsRequired = true)]
        public string ParameterName
        {
            get
            {
                return (string)this[Constants.ParameterName];
            }

            set
            {
                this[Constants.ParameterName] = (object)value;
            }
        }

        [ConfigurationProperty(Constants.ConnectionStringOrName, IsRequired = true)]
        public string ConnectionStringOrName
        {
            get
            {
                return (string)this[Constants.ConnectionStringOrName];
            }

            set
            {
                this[Constants.ConnectionStringOrName] = (object)value;
            }
        }

        public override string ElementName
        {
            get
            {
                return "destination";
            }
        }

        public override string Key
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public override IEnumerable<InjectionMember> GetInjectionMembers(IUnityContainer container, Type fromType, Type toType, string name)
        {
            return new[] { new DestinationDependentConnection(this.EndpointAddress, this.ParameterName, this.ConnectionStringOrName) };
        }
    }
}
