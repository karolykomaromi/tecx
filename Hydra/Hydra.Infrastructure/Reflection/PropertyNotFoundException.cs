namespace Hydra.Infrastructure.Reflection
{
    using System;

    public class PropertyNotFoundException : Exception
    {
        private readonly Type type;

        private readonly string propertyName;

        public PropertyNotFoundException(Type type, string propertyName)
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