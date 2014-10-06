namespace Hydra.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public static class TypeHelper
    {
        public static bool ImplementsOpenGenericInterface(Type implementationType, Type openGenericInterface)
        {
            Contract.Requires(implementationType != null);
            Contract.Requires(openGenericInterface != null);
            Contract.Requires(openGenericInterface.IsInterface);
            Contract.Requires(openGenericInterface.IsGenericType);
            Contract.Requires(openGenericInterface.IsGenericTypeDefinition);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            bool implementsOpenGenericInterface =
                implementationType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterface);

            return implementsOpenGenericInterface;
        }
    }
}