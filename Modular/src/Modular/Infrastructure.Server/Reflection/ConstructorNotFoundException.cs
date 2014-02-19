namespace Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;

    public class ConstructorNotFoundException : Exception
    {
        private readonly Type type;

        public ConstructorNotFoundException(Type type)
            : base(string.Format("No parameterless default constructor found for Type '{0}'.", type.AssemblyQualifiedName))
        {
            Contract.Requires(type != null);
            this.type = type;
        }

        public Type Type
        {
            get { return this.type; }
        }
    }
}