namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class DateTimeValueWriter : PropertyValueWriter
    {
        public DateTimeValueWriter(PropertyInfo property) 
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            DateTime dt;
            if (DateTime.TryParse(value, source, DateTimeStyles.None, out dt))
            {
                this.Property.SetValue(instance, dt);
            }
        }
    }
}