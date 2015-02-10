namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class CultureInfoValueWriter : PropertyValueWriter
    {
        public CultureInfoValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            CultureInfo culture;

            try
            {
                culture = new CultureInfo(value);
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new Error(string.Format(Properties.Resources.ErrorValueIsNotAKnownCulture, value));
            }

            try
            {
                this.Property.SetValue(target, culture);

                return ImportMessage.Empty;
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new Error(string.Format(Properties.Resources.ErrorWritingValue, culture, this.Property.Name));
            }
        }
    }
}