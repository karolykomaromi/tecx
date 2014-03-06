using System;
using System.Diagnostics.Contracts;

namespace Infrastructure.ListViews.Filter
{
    public class PropertyFilter : IPropertyFilter
    {
        private readonly string propertyName;

        public PropertyFilter(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.propertyName = propertyName;
        }

        public bool IsMatch(string propertyName)
        {
            bool isMatch = string.Equals(this.propertyName, propertyName, StringComparison.OrdinalIgnoreCase);
            return isMatch;
        }
    }
}