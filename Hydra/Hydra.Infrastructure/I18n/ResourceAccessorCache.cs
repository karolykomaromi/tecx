namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Reflection;

    [DebuggerDisplay("Count={accessors.Count}")]
    public class ResourceAccessorCache
    {
        public static readonly Func<string> EmptyAccessor = () => string.Empty;

        private readonly IDictionary<string, Func<string>> accessors;

        private readonly IFindPropertyConvention convention;

        public ResourceAccessorCache()
            : this(new CompositeConvention(new FindByTypeName(), new FindByTypeFullName()))
        {
        }

        public ResourceAccessorCache(IFindPropertyConvention convention)
        {
            Contract.Requires(convention != null);

            this.convention = convention;
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

                string resourceTypeName = assembly.GetName().Name + ".Properties.Resources";

                Type resourceType = assembly.GetType(resourceTypeName, false);

                if (resourceType == null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.accessors[accessorName] = accessor;
                    HydraEventSource.Log.ResourceTypeNotFound(assembly, resourceTypeName);
                    return accessor;
                }

                PropertyInfo property = this.convention.Find(resourceType, modelType, propertyName);

                if (property == Property.Null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.accessors[accessorName] = accessor;
                    HydraEventSource.Log.ResourcePropertyNotFound(assembly, resourceType, propertyName);
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
