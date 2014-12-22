namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class TypeValueWriter : PropertyValueWriter
    {
        public TypeValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            Type t;

            if (string.IsNullOrWhiteSpace(value))
            {
                t = typeof (Missing);
            }
            else
            {
                t = Type.GetType(value, false, true);
            }

            if (t == null)
            {
                return;
            }

            this.Property.SetValue(instance, t);
        }
    }
}