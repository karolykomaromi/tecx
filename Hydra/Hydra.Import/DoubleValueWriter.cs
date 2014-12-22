namespace Hydra.Import
{
    using System.Globalization;
    using System.Reflection;

    public class DoubleValueWriter : PropertyValueWriter
    {
        public DoubleValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            double d;
            if (double.TryParse(value, NumberStyles.Float, source, out d))
            {
                this.Property.SetValue(instance, d);
            }
        }
    }
}