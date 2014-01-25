namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    [DebuggerDisplay("key")]
    public struct ResxKey
    {
        public static readonly ResxKey Empty = new ResxKey("EMPTY_RESX_KEY");

        private readonly string key;

        public ResxKey(string key)
        {
            Contract.Requires(!string.IsNullOrEmpty(key));

            this.key = string.Intern(key.ToUpperInvariant());
        }

        public static bool operator ==(ResxKey x, ResxKey y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(ResxKey x, ResxKey y)
        {
            return !x.Equals(y);
        }

        public static ResxKey Create<T>(Expression<Func<T, string>> propertySelector)
        {
            return new ResxKey(ReflectionHelper.GetPropertyName(propertySelector));
        }

        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResxKey))
            {
                return false;
            }

            ResxKey other = (ResxKey)obj;

            return string.Equals(this.key, other.key, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.key;
        }
    }
}