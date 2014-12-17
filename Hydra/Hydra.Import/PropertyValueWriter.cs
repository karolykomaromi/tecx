namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public abstract class PropertyValueWriter : ValueWriter
    {
        private readonly PropertyInfo property;

        protected PropertyValueWriter(PropertyInfo property)
        {
            Contract.Requires(property != null);

            this.property = property;
        }

        public override string PropertyName
        {
            get { return this.Property.Name; }
        }

        protected PropertyInfo Property
        {
            get { return this.property; }
        }
    }
}