namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure.Logging;

    public class Int32ValueWriter : PropertyValueWriter
    {
        public Int32ValueWriter(PropertyInfo property)
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

            int i;
            if (int.TryParse(value, NumberStyles.Number | NumberStyles.Integer, sourceCulture, out i))
            {
                try
                {
                    this.Property.SetValue(target, i);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, i.ToString(CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(int).FullName));
        }
    }
}