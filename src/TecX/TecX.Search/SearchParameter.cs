namespace TecX.Search
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using TecX.Common;

    [DebuggerDisplay("Value={Value}")]
    public abstract class SearchParameter : IEquatable<SearchParameter>
    {
        private static readonly NullSearchParameter empty;

        static SearchParameter()
        {
            empty = new NullSearchParameter();
        }

        public static SearchParameter Empty
        {
            get
            {
                return empty;
            }
        }

        public abstract object Value { get; }

        public virtual bool Equals(SearchParameter other)
        {
            Guard.AssertNotNull(other, "other");

            return Equals(this.Value, other.Value);
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class SearchParameter<T> : SearchParameter
    {
        private readonly T value;

        public SearchParameter(T value)
        {
            this.value = value;
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
        }
    }
}