namespace TecX.Unity.Injection
{
    using TecX.Common;

    public class ConstructorArgument
    {
        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Guard.AssertNotNull(value, "Name");
                this.name = value;
            }
        }

        public object Value { get; set; }

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.name = argumentName;
            this.Value = value;
        }
    }
}