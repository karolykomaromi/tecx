using Infrastructure.Reflection;

namespace Infrastructure.Options
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    [DebuggerDisplay("{name}")]
    public struct Option
    {
        public static readonly Option Empty = new Option("EMPTY_OPTION");

        private readonly string name;

        private Option(string key)
        {
            Contract.Requires(!string.IsNullOrEmpty(key));

            this.name = string.Intern(key.ToUpperInvariant());
        }

        public static bool operator ==(Option x, Option y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Option x, Option y)
        {
            return !x.Equals(y);
        }

        public static Option Create<TOption, TProperty>(Expression<Func<TOption, TProperty>> propertySelector)
            where TOption : IOptions
        {
            return new Option(typeof(TOption).FullName + "." + ReflectionHelper.GetPropertyName(propertySelector));
        }

        public static Option Create<TOption, TProperty>(TOption options, Expression<Func<TOption, TProperty>> propertySelector)
            where TOption : IOptions
        {
            return new Option(typeof(TOption).FullName + "." + ReflectionHelper.GetPropertyName(propertySelector));
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Option))
            {
                return false;
            }

            Option other = (Option)obj;

            return string.Equals(this.name, other.name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.name;
        }

        public string GetTypeName()
        {
            string optionName = this.ToString();

            int idx = optionName.LastIndexOf(".", StringComparison.Ordinal);

            string typeName = optionName.Substring(0, idx);

            return typeName;
        }

        public string GetPropertyName()
        {
            string propertyName = this.ToString();

            int idx = propertyName.LastIndexOf(".", StringComparison.Ordinal);

            propertyName = propertyName.Substring(idx + 1);

            return propertyName;
        }
    }
}