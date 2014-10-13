namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    [ContractClass(typeof(ResxPropertyConventionContract))]
    public interface IResxPropertyConvention
    {
        PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName);
    }

    [ContractClassFor(typeof(IResxPropertyConvention))]
    internal abstract class ResxPropertyConventionContract : IResxPropertyConvention
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