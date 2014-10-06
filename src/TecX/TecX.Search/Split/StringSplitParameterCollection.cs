namespace TecX.Search.Split
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class StringSplitParameterCollection : IEnumerable<StringSplitParameter>
    {
        private readonly List<StringSplitParameter> parameters;

        public StringSplitParameterCollection()
        {
            this.parameters = new List<StringSplitParameter>();
        }

        public int Count
        {
            get
            {
                return this.parameters.Count;
            }
        }

        public StringSplitParameter this[int index]
        {
            get
            {
                return this.parameters[index];
            }
        }

        public void Add(StringSplitParameter parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            if (!this.parameters.Contains(parameter))
            {
                this.parameters.Add(parameter);
            }
        }

        public string[] ToArray()
        {
            return this.parameters.Select(p => p.Value).ToArray();
        }

        public IEnumerator<StringSplitParameter> GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Clear()
        {
            this.parameters.Clear();
        }

        public bool Contains(StringSplitParameter parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            return this.parameters.Contains(parameter);
        }
    }
}