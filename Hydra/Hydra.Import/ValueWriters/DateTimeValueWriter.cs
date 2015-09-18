namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Logging;

    public class DateTimeValueWriter : PropertyValueWriter
    {
        public DateTimeValueWriter(PropertyInfo property) 
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

            DateTime dt;
            if (DateTime.TryParse(value, sourceCulture, DateTimeStyles.None, out dt))
            {
                try
                {
                    this.Property.SetValue(target, dt);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, dt.ToString(FormatStrings.DateAndTime.RoundTrip, CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }
            
            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(DateTime).FullName));
        }
    }
}