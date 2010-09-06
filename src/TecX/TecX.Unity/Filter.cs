using System;

namespace TecX.Unity
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
            if (filter == null) throw new ArgumentNullException("filter");
            if (description == null) throw new ArgumentNullException("description");

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
