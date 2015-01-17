namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    public class CompositeConvention : IResXPropertyConvention
    {
        private readonly HashSet<IResXPropertyConvention> conventions;

        public CompositeConvention(params IResXPropertyConvention[] conventions)
        {
            this.conventions = new HashSet<IResXPropertyConvention>(conventions ?? new IResXPropertyConvention[0]);
        }

        public PropertyInfo FindProperty(Type resourceType, Type modelType, string propertyName)
        {
            foreach (IResXPropertyConvention convention in this.conventions)
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