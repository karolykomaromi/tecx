namespace Hydra.Import
{
    using System.Globalization;
    using System.Reflection;

    public class Int32ValueWriter : PropertyValueWriter
    {
        public Int32ValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            int i;
            if (int.TryParse(value, NumberStyles.Number | NumberStyles.Integer, source, out i))
            {
                this.Property.SetValue(instance, i);
            }
        }
    }
}