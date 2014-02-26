namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    public class ServiceClientConvention : RegistrationConvention
    {
        protected override void Register(IUnityContainer container, Type type)
        {
            container.RegisterType(type, GetServiceClassType(type));
        }

        protected override bool IsMatch(Type type)
        {
            bool isMatch = IsServiceContract(type) && GetServiceClassType(type) != null;

            return isMatch;
        }

        private static bool IsServiceContract(Type type)
        {
            return type.IsInterface && 
                   type.Name.EndsWith("Service", StringComparison.Ordinal) && 
                   type.GetCustomAttributes(typeof(ServiceContractAttribute), false) != null;
        }

        private static Type GetServiceClassType(Type contract)
        {
            string serviceClassName = contract.Name.Substring(1) + "Client";
            string serviceClassFullName = contract.FullName.Replace(contract.Name, serviceClassName);

            return contract.Assembly.GetType(serviceClassFullName, false);
        }
    }
}
