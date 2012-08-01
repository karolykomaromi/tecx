namespace TecX.Unity.Injection
{
    using TecX.Common;

    public class ConstructorArgument
    {
        private readonly NamingConvention convention;

        private readonly object value;

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.value = value;

            this.convention = new SpecifiedNameConvention(argumentName);
        }

        public ConstructorArgument(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.value = value;

            this.convention = NamingConvention.Create(value.GetType());
        }

        public object Value
        {
            get
            {
                return this.value;
            }
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.convention.NameMatches(name);
        }
    }
}