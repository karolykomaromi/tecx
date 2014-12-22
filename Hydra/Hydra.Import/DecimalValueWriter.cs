namespace Hydra.Import
{
    using System.Globalization;
    using System.Reflection;

    public class DecimalValueWriter : PropertyValueWriter
    {
        public DecimalValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            decimal d;
            if (decimal.TryParse(value, NumberStyles.Float, source, out d))
            {
                this.Property.SetValue(instance, d);
            }
        }
    }
}