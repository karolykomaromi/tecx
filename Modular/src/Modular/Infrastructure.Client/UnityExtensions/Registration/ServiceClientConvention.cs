namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    public class ServiceClientConvention : RegistrationConvention
    {
        protected override void Register(IUnityContainer container, Type type)
        {
            container.RegisterType(GetServiceContract(type), type);
        }

        protected override bool IsMatch(Type type)
        {
            bool isMatch = type.Name.EndsWith("ServiceClient", StringComparison.Ordinal) && ImplementsServiceContract(type);

            return isMatch;
        }

        private static bool ImplementsServiceContract(Type type)
        {
            Contract.Requires(type != null);

            if (type.IsAbstract || type.IsInterface)
            {
                return false;
            }

            Type contract = GetServiceContract(type);

            return contract != null;
        }

        private static Type GetServiceContract(Type type)
        {
            Type[] interfaces = type.GetInterfaces();

            Type contract = interfaces
                .OrderBy(i => i.FullName, StringComparer.Ordinal)
                .FirstOrDefault(c => c.GetCustomAttributes(typeof(ServiceContractAttribute), true) != null);

            return contract;
        }
    }
}
