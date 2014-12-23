namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class FloatValueWriter : PropertyValueWriter
    {
        public FloatValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            float f;
            if (float.TryParse(value, NumberStyles.Float | NumberStyles.Number, source, out f))
            {
                try
                {
                    this.Property.SetValue(instance, f);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, f.ToString("R", CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(float).FullName));
        }
    }
}