namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Logging;

    public class FloatValueWriter : PropertyValueWriter
    {
        public FloatValueWriter(PropertyInfo property)
            : base(property)
        {
            Contract.Requires(property != null);
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            float f;
            if (float.TryParse(value, NumberStyles.Float | NumberStyles.Number, sourceCulture, out f))
            {
                try
                {
                    this.Property.SetValue(target, f);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, f.ToString(FormatStrings.Numeric.RoundTrip, CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(float).FullName));
        }
    }
}