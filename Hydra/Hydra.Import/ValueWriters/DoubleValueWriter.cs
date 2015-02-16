namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure.Logging;

    public class DoubleValueWriter : PropertyValueWriter
    {
        public DoubleValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            double d;
            if (double.TryParse(value, NumberStyles.Float | NumberStyles.Number, sourceCulture, out d))
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

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(double).FullName));
        }
    }
}