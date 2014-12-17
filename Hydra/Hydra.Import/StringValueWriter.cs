namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;

    public class StringValueWriter : PropertyValueWriter
    {
        public StringValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            Contract.Requires(instance != null);

            this.Property.SetValue(instance, value);
        }
    }
}