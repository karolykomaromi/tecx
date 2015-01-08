namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class DecimalValueWriter : PropertyValueWriter
    {
        public DecimalValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            decimal d;
            if (decimal.TryParse(value, NumberStyles.Float | NumberStyles.Number, sourceCulture, out d))
            {
                try
                {
                    this.Property.SetValue(target, d);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, d.ToString("R", CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(decimal).FullName));
        }
    }
}