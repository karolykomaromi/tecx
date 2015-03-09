namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure.Logging;

    public class Int64ValueWriter : PropertyValueWriter
    {
        public Int64ValueWriter(PropertyInfo property)
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

            long l;
            if (long.TryParse(value, NumberStyles.Number | NumberStyles.Integer, sourceCulture, out l))
            {
                try
                {
                    this.Property.SetValue(target, l);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, l.ToString(CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(long).FullName));
        }
    }
}