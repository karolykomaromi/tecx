namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;

    [DebuggerDisplay("Count={Count}")]
    public class CompositeConvention : IResXPropertyConvention
    {
        private readonly HashSet<IResXPropertyConvention> conventions;

        public CompositeConvention(params IResXPropertyConvention[] conventions)
        {
            this.conventions = new HashSet<IResXPropertyConvention>();

            this.AddRange(conventions);
        }

        public int Count
        {
            get { return this.conventions.Count; }
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

        private void AddRange(IEnumerable<IResXPropertyConvention> conventions)
        {
            if (conventions == null)
            {
                return;
            }

            foreach (var convention in conventions)
            {
                if (object.ReferenceEquals(this, convention))
                {
                    continue;
                }

                CompositeConvention other = convention as CompositeConvention;

                if (other != null)
                {
                    this.AddRange(other.conventions);
                }
                else
                {
                    this.conventions.Add(convention);
                }
            }
        }
    }
}