namespace Infrastructure.ListViews.Filter
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class CompositeFilter : IPropertyFilter
    {
        private readonly List<IPropertyFilter> filters;

        public CompositeFilter(params IPropertyFilter[] filters)
        {
            this.filters = new List<IPropertyFilter>(filters ?? new IPropertyFilter[0]);
        }

        public bool IsMatch(string propertyName)
        {
            bool isMatch = this.filters.Any(f => f.IsMatch(propertyName));

            return isMatch;
        }

        public void Add(IPropertyFilter filter)
        {
            Contract.Requires(filter != null);

            this.filters.Add(filter);
        }
    }
}