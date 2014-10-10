namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    public class CompositeConvention : IFindPropertyConvention
    {
        private readonly HashSet<IFindPropertyConvention> conventions;

        public CompositeConvention(params IFindPropertyConvention[] conventions)
        {
            this.conventions = new HashSet<IFindPropertyConvention>(conventions ?? new IFindPropertyConvention[0]);
        }

        public PropertyInfo Find(Type resourceType, Type modelType, string propertyName)
        {
            foreach (IFindPropertyConvention convention in this.conventions)
            {
                PropertyInfo property;
                if ((property = convention.Find(resourceType, modelType, propertyName)) != Property.Null)
                {
                    return property;
                }
            }

            return Property.Null;
        }
    }
}