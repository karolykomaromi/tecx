namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    [ContractClass(typeof(ResXPropertyConventionContract))]
    public interface IResXPropertyConvention
    {
        PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName);
    }

    [ContractClassFor(typeof(IResXPropertyConvention))]
    internal abstract class ResXPropertyConventionContract : IResXPropertyConvention
    {
        public PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(modelType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            Contract.Ensures(Contract.Result<PropertyInfo>() != null);

            return Property.Null;
        }
    }
}