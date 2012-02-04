namespace TecX.Unity.Injection
{
    using System;

    using TecX.Common;

    public class ConstructorArgument
    {
        private readonly Func<string, bool> nameMatches;

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.nameMatches = name => string.Equals(argumentName, name, StringComparison.InvariantCultureIgnoreCase);
            this.Value = value;
        }

        public ConstructorArgument(object value)
        {
            Guard.AssertNotNull(value, "value");
            this.Value = value;
            this.nameMatches = new DefaultNamingConvention(value.GetType()).NameMatches;
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.nameMatches(name);
        }

        public object Value { get; set; }
    }

    public class DefaultNamingConvention
    {
        private readonly Type type;

        public DefaultNamingConvention(Type type)
        {
            Guard.AssertNotNull(type, "type");
            this.type = type;
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return false;
        }
    }
}