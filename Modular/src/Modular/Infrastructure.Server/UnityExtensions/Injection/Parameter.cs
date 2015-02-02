namespace Infrastructure.UnityExtensions.Injection
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("Name:'{Name}' Value:'{Value}'")]
    public class Parameter
    {
        private readonly object value;

        private readonly string name;

        public Parameter(string name, object value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            this.value = value;
            this.name = name;
        }

        public Parameter(object value)
        {
            Contract.Requires(value != null);

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