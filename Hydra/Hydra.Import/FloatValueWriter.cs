namespace Hydra.Import
{
    using System.Globalization;
    using System.Reflection;

    public class FloatValueWriter : PropertyValueWriter
    {
        public FloatValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            float f;
            if (float.TryParse(value, NumberStyles.Float | NumberStyles.Number, source, out f))
            {
                this.Property.SetValue(instance, f);
            }
        }
    }
}