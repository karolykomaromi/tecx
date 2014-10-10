namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    public class FindByTypeFullName : IFindPropertyConvention
    {
        public PropertyInfo Find(Type resourceType, Type modelType, string propertyName)
        {
            PropertyInfo property = resourceType.GetProperty(modelType.FullName.Replace(".", "_") + "_" + propertyName, BindingFlags.Public | BindingFlags.Static);

            if (property != null)
            {
                return property;
            }

            return Property.Null;
        }
    }
}