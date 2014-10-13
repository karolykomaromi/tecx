namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    public class FindByTypeFullName : IResxPropertyConvention
    {
        public PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName)
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