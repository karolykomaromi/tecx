namespace Infrastructure.Options
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    [DebuggerDisplay("{name}")]
    public struct OptionName
    {
        public static readonly OptionName Empty = new OptionName("EMPTY_OPTION_NAME");

        private readonly string name;

        public OptionName(string key)
        {
            Contract.Requires(!string.IsNullOrEmpty(key));

            this.name = string.Intern(key.ToUpperInvariant());
        }

        public static bool operator ==(OptionName x, OptionName y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(OptionName x, OptionName y)
        {
            return !x.Equals(y);
        }

        public static OptionName Create<T>(Expression<Func<T, string>> propertySelector)
        {
            return new OptionName(ReflectionHelper.GetPropertyName(propertySelector));
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OptionName))
            {
                return false;
            }

            OptionName other = (OptionName)obj;

            return string.Equals(this.name, other.name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}