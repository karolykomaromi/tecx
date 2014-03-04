using System;

namespace Infrastructure.ListViews.Filter
{
    public class IdFilter : IPropertyFilter
    {
        public bool IsMatch(string propertyName)
        {
            bool isMatch = string.Equals("Id", propertyName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }
    }
}