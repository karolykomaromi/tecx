namespace Infrastructure.Reflection
{
    using System;
    using System.Globalization;

    public class PropertyNotFoundException : Exception
    {
        private readonly Type type;

        private readonly string propertyName;

        public PropertyNotFoundException(Type type, string propertyName)
            : base(string.Format(CultureInfo.CurrentCulture, "No public static property named '{0}' found in class of Type '{1}'.", propertyName, type.AssemblyQualifiedName))
        {
            this.type = type;
            this.propertyName = propertyName;
        }

        public Type Type
        {
            get { return this.type; }
        }

        public string PropertyName
        {
            get { return this.propertyName; }
        }
    }
}