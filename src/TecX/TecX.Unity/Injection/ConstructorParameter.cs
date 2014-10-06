namespace TecX.Unity.Injection
{
    using System.Diagnostics;

    using TecX.Common;

    [DebuggerDisplay("Name:'{Name}' Value:'{Value}'")]
    public class ConstructorParameter
    {
        private readonly object value;

        private readonly string name;

        public ConstructorParameter(string name, object value)
        {
            Guard.AssertNotEmpty(name, "name");

            this.value = value;
            this.name = name;
        }

        public ConstructorParameter(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.value = value;
            this.name = string.Empty;
        }

        public object Value
        {
            get
            {
                return this.value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}