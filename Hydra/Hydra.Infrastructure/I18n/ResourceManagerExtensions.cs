namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq.Expressions;
    using Hydra.Infrastructure.Reflection;

    public static class ResourceManagerExtensions
    {
        public static string GetString(this IResourceManager resourceManager, Expression<Func<string>> propertySelector, CultureInfo culture)
        {
            Contract.Requires(resourceManager != null);
            Contract.Requires(propertySelector != null);

            string propertyName = TypeHelper.GetPropertyName(propertySelector);

            return resourceManager.GetString(propertyName, culture ?? CultureInfo.CurrentUICulture);
        }
    }
}
