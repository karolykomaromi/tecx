using TecX.Common;

namespace TecX.Unity.Injection
{
    public class ConstructorArgument
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Guard.AssertNotNull(value, "Name");
                _name = value;
            }
        }

        public object Value { get; set; }

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            _name = argumentName;
            Value = value;
        }
    }
}