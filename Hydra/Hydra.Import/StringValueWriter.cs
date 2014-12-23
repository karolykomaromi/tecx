namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class StringValueWriter : PropertyValueWriter
    {
        public StringValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (value == null)
            {
                return ImportMessage.Empty;
            }

            try
            {
                this.Property.SetValue(instance, value.ToString(target));
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new Error(string.Format(Properties.Resources.ErrorWritingValue, value.ToString(target), this.Property.Name));
            }

            return ImportMessage.Empty;
        }
    }
}