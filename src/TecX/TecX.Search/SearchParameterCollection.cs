namespace TecX.Search
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class SearchParameterCollection : IEnumerable<SearchParameter>
    {
        private readonly List<SearchParameter> searchParameters;

        public SearchParameterCollection()
        {
            this.searchParameters = new List<SearchParameter>();
        }

        public int Count
        {
            get
            {
                return this.searchParameters.Count;
            }
        }

        public SearchParameter this[int index]
        {
            get
            {
                return this.searchParameters[index];
            }

            set
            {
                this.searchParameters[index] = value;
            }
        }

        public IEnumerator<SearchParameter> GetEnumerator()
        {
            return this.searchParameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(SearchParameter item)
        {
            Guard.AssertNotNull(item, "item");

            this.searchParameters.Add(item);
        }

        public bool Contains(SearchParameter item)
        {
            Guard.AssertNotNull(item, "item");

            return this.searchParameters.Contains(item);
        }

        public bool Remove(SearchParameter item)
        {
            Guard.AssertNotNull(item, "item");

            return this.searchParameters.Remove(item);
        }

        public object[] GetParameterValues()
        {
            var values = this.searchParameters.Select(sp => sp.Value).ToArray();

            return values;
        }

        public void Clear()
        {
            this.searchParameters.Clear();
        }
    }
}