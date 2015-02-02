namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;

    public class PropertyNotFoundException : Exception
    {
        private readonly Type type;

        private readonly string propertyName;

        public PropertyNotFoundException(Type type, string propertyName)
        {
            Contract.Requires(type != null);

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