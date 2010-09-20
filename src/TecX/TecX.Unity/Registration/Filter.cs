using System;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class Filter<T>
        where T : class
    {
        private readonly Predicate<T> _filter;
        private readonly string _description;

        public string Description
        {
            get { return _description; }
        }

        public Filter(Predicate<T> filter, string description)
        {
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotEmpty(description, "description");

            _filter = filter;
            _description = description;
        }

        public bool IsMatch(T item)
        {
            if (item == null) throw new ArgumentNullException("item");

            return _filter(item);
        }
    }
}
