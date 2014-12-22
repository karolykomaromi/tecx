namespace Hydra.Import
{
    using System.Globalization;
    using System.Reflection;

    public class Int64ValueWriter : PropertyValueWriter
    {
        public Int64ValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            long l;
            if (long.TryParse(value, NumberStyles.Number | NumberStyles.Integer, source, out l))
            {
                this.Property.SetValue(instance, l);
            }
        }
    }
}