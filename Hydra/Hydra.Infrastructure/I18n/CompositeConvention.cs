namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    public class CompositeConvention : IResxPropertyConvention
    {
        private readonly HashSet<IResxPropertyConvention> conventions;

        public CompositeConvention(params IResxPropertyConvention[] conventions)
        {
            this.conventions = new HashSet<IResxPropertyConvention>(conventions ?? new IResxPropertyConvention[0]);
        }

        public PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName)
        {
            foreach (IResxPropertyConvention convention in this.conventions)
            {
                PropertyInfo property;
                if ((property = convention.FindProperty(resourceType, modelType, propertyName)) != Property.Null)
                {
                    return property;
                }
            }

            return Property.Null;
        }
    }
}