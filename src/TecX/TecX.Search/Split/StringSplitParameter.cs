namespace TecX.Search.Split
{
    using System;
    using System.Diagnostics;

    using TecX.Common;

    [DebuggerDisplay("Value={Value}")]
    public class StringSplitParameter : IEquatable<StringSplitParameter>
    {
        private string value;

        public StringSplitParameter(string value)
        {
            Guard.AssertNotEmpty(value, "value");

            this.value = value;
        }

        protected StringSplitParameter()
        {
        }

        public virtual string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                Guard.AssertNotEmpty(value, "Value");

                this.value = value;
            }
        }

        public bool Equals(StringSplitParameter other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
