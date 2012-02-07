namespace TecX.Unity.Injection
{
    using TecX.Common;

    public class ConstructorArgument
    {
        private readonly NamingConvention convention;

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.Value = value;

            this.convention = new SpecifiedNameConvention(argumentName);
        }

        public ConstructorArgument(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.Value = value;

            this.convention = NamingConvention.CreateForType(value.GetType());
        }

        public object Value { get; private set; }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.convention.NameMatches(name);
        }
    }
}