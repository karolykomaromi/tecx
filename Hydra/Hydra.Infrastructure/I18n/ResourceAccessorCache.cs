namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    [DebuggerDisplay("Count={accessors.Count}")]
    public class ResourceAccessorCache
    {
        public static readonly Func<string> EmptyAccessor = () => string.Empty;

        private readonly IDictionary<string, Func<string>> accessors;

        public ResourceAccessorCache()
        {
            this.accessors = new Dictionary<string, Func<string>>(StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyDictionary<string, Func<string>> Accessors
        {
            get { return new ReadOnlyDictionary<string, Func<string>>(this.accessors); }
        }

        public Func<string> GetAccessor(Type modelType, string propertyName)
        {
            Contract.Requires(modelType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            Contract.Ensures(Contract.Result<Func<string>>() != null);

            string accessorName = modelType.FullName.Replace('.', '_') + "_" + propertyName;

            Func<string> accessor;
            if (!this.Accessors.TryGetValue(accessorName, out accessor))
            {
                Assembly assembly = modelType.Assembly;

                Type resourceType = assembly.GetType(assembly.GetName().Name + ".Properties.Resources", false);

                if (resourceType == null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.accessors[accessorName] = accessor;
                    return accessor;
                }

                PropertyInfo property = resourceType.GetProperty(modelType.Name + "_" + propertyName, BindingFlags.Public | BindingFlags.Static);

                if (property == null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.accessors[accessorName] = accessor;
                    return accessor;
                }

                MemberExpression expression = Expression.Property(null, property);

                Expression<Func<string>> lambda = Expression.Lambda<Func<string>>(expression);

                accessor = lambda.Compile();
                this.accessors[accessorName] = accessor;
            }

            return accessor;
        }

        public void Clear()
        {
            this.accessors.Clear();
        }
    }
}
