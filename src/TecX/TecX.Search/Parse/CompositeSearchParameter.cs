namespace TecX.Search.Parse
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    using TecX.Common;

    public abstract class CompositeSearchParameter : SearchParameter, IEnumerable<SearchParameter>
    {
        private readonly List<SearchParameter> parameters;

        public CompositeSearchParameter()
        {
            this.parameters = new List<SearchParameter>();
        }

        public override object Value
        {
            get
            {
                return this.parameters.Select(p => p.Value).ToArray();
            }
        }

        public void Add(SearchParameter parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            this.parameters.Add(parameter);
        }

        public IEnumerator<SearchParameter> GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}