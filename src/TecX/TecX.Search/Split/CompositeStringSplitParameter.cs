namespace TecX.Search.Split
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    using TecX.Common;
    using TecX.Search;

    public class CompositeStringSplitParameter : StringSplitParameter, IEnumerable<StringSplitParameter>
    {
        private readonly List<StringSplitParameter> parameters;

        public CompositeStringSplitParameter()
        {
            this.parameters = new List<StringSplitParameter>();
        }

        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                for (int i = 0; i < this.parameters.Count; i++)
                {
                    if (i < this.parameters.Count - 1)
                    {
                        sb.Append(this.parameters[i].Value).Append(Constants.Blank);
                    }
                    else
                    {
                        sb.Append(this.parameters[i].Value);
                    }
                }

                return sb.ToString();
            }

            set
            {
                throw new InvalidOperationException("Cannot set value of composite parameter.");
            }
        }

        public void Add(StringSplitParameter parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            this.parameters.Add(parameter);
        }

        public IEnumerator<StringSplitParameter> GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}