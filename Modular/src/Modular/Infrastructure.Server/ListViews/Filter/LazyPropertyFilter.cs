namespace Infrastructure.ListViews.Filter
{
    using System;

    public class LazyPropertyFilter : IPropertyFilter
    {
        private readonly Lazy<IPropertyFilter> lazy;

        public LazyPropertyFilter(Func<IPropertyFilter> valueFactory)
        {
            this.lazy = new Lazy<IPropertyFilter>(valueFactory);
        }

        public bool IsMatch(string propertyName)
        {
            return this.lazy.Value.IsMatch(propertyName);
        }
    }
}
